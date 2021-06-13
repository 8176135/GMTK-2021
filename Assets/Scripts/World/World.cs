using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    [Header("WorldSeed")]
    public bool useRandomSeed;
    public int seed;
    
    [Header("Terrain max passes")]
    [Range(1, 128)]
    public int maxPasses = 1;

    private GameObject[] obstacles;
    private Rect[] allRects;
    
    
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

        var halfSize = (float) size / 2;
        var halfWallWidth = wallWidth / 2;
        
        wallNorth.position = new Vector3(0f, halfSize + halfWallWidth);
        wallSouth.position = new Vector3(0f, -halfSize - halfWallWidth);
        wallEast.position = new Vector3(halfSize + halfWallWidth, 0f);
        wallWest.position = new Vector3(-halfSize - halfWallWidth, 0f);

        // This is meant to prevent the obstacles from being placed in the wall
        // But I've run out of patience to fix it atm
        allRects = new[]
        {
            new Rect(-halfSize, halfSize + halfWallWidth, size, wallWidth),
            new Rect(-halfSize, -(halfSize + halfWallWidth), size, wallWidth),
            new Rect(-(halfSize + halfWallWidth), halfSize, wallWidth, size),
            new Rect(halfSize + halfWallWidth, halfSize, wallWidth, size)
        };

        wallNorth.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallSouth.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallEast.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);
        wallWest.localScale = new Vector3(size + (wallWidth * 2), wallWidth, 1f);

        PlaceTerrain();
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
                placedObstacles.Add(spawned);
                placedObRectangles.Add(new Rect(x - halfScale, y - halfScale, scale, scale));
                
                success++;
            }
        }

        obstacles = placedObstacles.ToArray();
    }

    public void CleanTerrain()
    {
        if (obstacles == null) return;
        
        foreach (var ob in obstacles)
        {
            DestroyImmediate(ob);
        }

        obstacles = new GameObject[0];
    }
}
