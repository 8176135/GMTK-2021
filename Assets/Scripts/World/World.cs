using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    [Header("Wall ref")]
    public Transform wallNorth;
    public Transform wallSouth;
    public Transform wallEast;
    public Transform wallWest;

    [Header("Obstacle ref")]
    public GameObject obstacle;
    
    [Header("World Limits")]
    public int size = 2048;
    public float wallWidth = 64f;
    public bool autoUpdate;
    
    [Header("World Seed")]
    public bool useRandomSeed;
    public int seed;

    [Header("Block list")]
    public BlockPlaceInfo[] blocksToPlace;
    
    [Header("Terrain max passes")]
    [Range(1, 128)]
    public int maxPasses = 1;

    private GameObject[] obstacles;
    private GameObject[] blocks;
    private Rect[] allRects = new Rect[0];
    
    
    // Start is called before the first frame update
    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        if (useRandomSeed)
        {
            seed = Random.Range(-100000, 100000);
        }
        
        Random.InitState(seed);

        CleanTerrain();
        CleanBlocks();

        var halfSize = (float) size / 2;
        var halfWallWidth = wallWidth / 2;
        
        wallNorth.position = new Vector3(0f, halfSize + halfWallWidth);
        wallSouth.position = new Vector3(0f, -halfSize - halfWallWidth);
        wallEast.position = new Vector3(halfSize + halfWallWidth, 0f);
        wallWest.position = new Vector3(-halfSize - halfWallWidth, 0f);

        // This is meant to prevent the obstacles from being placed in the wall
        // But I've run out of patience to fix it atm
        // allRects = new[]
        // {
        //     new Rect(-halfSize, halfSize + halfWallWidth, size, wallWidth),
        //     new Rect(-halfSize, -(halfSize + halfWallWidth), size, wallWidth),
        //     new Rect(-(halfSize + halfWallWidth), halfSize, wallWidth, size),
        //     new Rect(halfSize + halfWallWidth, halfSize, wallWidth, size)
        // };

        wallNorth.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallSouth.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallEast.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallWest.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);

        PlaceTerrain();
        PlaceBlocks();
    }

    public void PlaceTerrain()
    {
        var placedObstacles = new List<GameObject>();
        var placedObRectangles = new List<Rect>();
        placedObRectangles.AddRange(allRects);
        
        var halfSize = (float) size / 2;
        
        for (int i = 1; i < maxPasses + 1; i++)
        {
            var scale = Mathf.Lerp(((float) size / 10) - i,  1f, (float) i / (maxPasses + 1));
            var halfScale = scale / 2;

            var fails = 0;
            var success = 0;

            while (fails < i && success < i)
            {
                var x = Random.Range(-halfSize, halfSize);
                var y = Random.Range(-halfSize, halfSize);

                var thisRect = new Rect(x - halfScale, y - halfScale, scale, scale);

                var isOverlap = false;
                foreach (var placedRect in placedObRectangles)
                {
                    if (!placedRect.Overlaps(thisRect)) continue;
                    
                    isOverlap = true;
                    break;
                }
                
                if (isOverlap)
                {
                    fails++;
                    continue;
                }
                
                var spawned = Instantiate(obstacle, new Vector3(x, y, 0f), Quaternion.identity);
                spawned.transform.localScale = new Vector3(scale, scale, 1f);

                var angle = Random.Range(0, 360);
                if (angle < 90) angle = 0;
                if (angle >= 90 && angle < 180) angle = 90;
                if (angle >= 180 && angle < 270) angle = 180;
                if (angle >= 270 && angle < 360) angle = 270;
                
                spawned.transform.Rotate(0, 0, angle);
                placedObstacles.Add(spawned);
                placedObRectangles.Add(new Rect(x - halfScale, y - halfScale, scale, scale));
                
                success++;
            }
        }

        obstacles = placedObstacles.ToArray();
    }

    public void CleanTerrain()
    {
        if (obstacles == null)
        {
            obstacles = new GameObject[0];
            return;
        }
        
        foreach (var ob in obstacles)
        {
            DestroyImmediate(ob);
        }

        obstacles = new GameObject[0];
    }

    public void CleanBlocks()
    {
        if (blocks == null)
        {
            blocks = new GameObject[0];
            return;
        }
        
        foreach (var bl in blocks)
        {
            DestroyImmediate(bl);
        }

        blocks = new GameObject[0];
    }
    
    public void PlaceBlocks()
    {
        var halfSize = (float) size / 2;
        var placedBlocks = new List<GameObject>();
        var placedBlockRects = new List<Rect>();
        
        var scale = 1f;
        var halfScale = scale / 2;
        
        foreach (var blockData in blocksToPlace)
        {
            for (int i = 1; i < blockData.maxPasses + 1; i++)
            {
                var fails = 0;
                var success = 0;

                while (fails < i && success < i)
                {
                    var x = Random.Range(-halfSize, halfSize);
                    var y = Random.Range(-halfSize, halfSize);

                    var thisRect = new Rect(x - halfScale, y - halfScale, 1, 1);

                    var isOverlap = false;
                    // For each in these rects
                    foreach (var placedRect in placedBlockRects)
                    {
                        if (!placedRect.Overlaps(thisRect)) continue;
                    
                        isOverlap = true;
                        break;
                    }
                    
                    // For each in all rects
                    foreach (var rect in allRects)
                    {
                        if (!rect.Overlaps(thisRect)) continue;
                    
                        isOverlap = true;
                        break;
                    }
                
                    if (isOverlap)
                    {
                        fails++;
                        continue;
                    }
                
                    var spawned = Instantiate(blockData.block, new Vector3(x, y, 0f), Quaternion.identity);

                    var angle = Random.Range(0, 360);
                    if (angle < 90) angle = 0;
                    if (angle >= 90 && angle < 180) angle = 90;
                    if (angle >= 180 && angle < 270) angle = 180;
                    if (angle >= 270 && angle < 360) angle = 270;
                
                    spawned.transform.Rotate(0, 0, angle);
                    placedBlocks.Add(spawned);
                    placedBlockRects.Add(new Rect(x - halfScale, y - halfScale, 1, 1));
                
                    success++;
                }
            }
        }

        blocks = placedBlocks.ToArray();

        var temp = new List<Rect>(allRects);
        temp.AddRange(placedBlockRects.ToArray());
        allRects = temp.ToArray();
    }
}
