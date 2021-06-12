using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap, float clamp)
    {
        var width = noiseMap.GetLength(0);
        var height = noiseMap.GetLength(1);
        
        Debug.Log($"Width: {width}, Height: {height}");

        var texture = new Texture2D(width, height);

        var colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        
        texture.SetPixels(colorMap);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        if (textureRender is SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        }
        
        textureRender.transform.localScale = new Vector3(width, height, 0);

        var collider = GetComponent<PolygonCollider2D>();
        var points = new List<Vector2>();
        
        var checkedPoints = new bool[width][];
        for (int index = 0; index < width; index++)
        {
            checkedPoints[index] = new bool[height];
        }

        var buckets = new List<Vector2[]>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (checkedPoints[x][y]) continue;
                var bucket = new List<Vector2>();
                CheckNeighbourPoints(ref checkedPoints, ref bucket, noiseMap, x, y, clamp);

                if (bucket.Count > 2)
                {
                    buckets.Add(bucket.ToArray());
                }
            }
        }

        var paths = buckets.ToArray();

        var halfWidth = (float) width / 2;
        var halfHeight = (float) height / 2;
        
        foreach (var t in paths)
        {
            for (int j = 0; j < t.Length; j++)
            {
                var point = t[j];
                t[j] = new Vector2(point.x / width - 0.5f, point.y / height - 0.5f);
            }
        }

        collider.pathCount = paths.Length;
        for (int i = 0; i < paths.Length; i++)
        {
            collider.SetPath(i, paths[i]);
        }
    }

    private void CheckNeighbourPoints(ref bool[][] checkedPoints, ref List<Vector2> bucket, float[,] noiseMap, int x, int y, float clamp)
    {
        checkedPoints[x][y] = true;
        if (noiseMap[x, y] < clamp) return;
        
        bucket.Add(new Vector2(x, y));
        
        if (x != checkedPoints.Length - 1)
        {
            if (IsValidPoint(ref checkedPoints, noiseMap, x + 1, y, clamp))
            {
                bucket.Add(new Vector2(x + 1, y));
                CheckNeighbourPoints(ref checkedPoints, ref bucket, noiseMap, x + 1, y, clamp);
            }
        }
        
        if (x != 0)
        {
            if (IsValidPoint(ref checkedPoints, noiseMap, x - 1, y, clamp))
            {
                bucket.Add(new Vector2(x - 1, y));
                CheckNeighbourPoints(ref checkedPoints, ref bucket, noiseMap, x - 1, y, clamp);
            }
        }
        
        if (y != checkedPoints[0].Length - 1)
        {
            if (IsValidPoint(ref checkedPoints, noiseMap, x, y + 1, clamp))
            {
                bucket.Add(new Vector2(x, y + 1));
                CheckNeighbourPoints(ref checkedPoints, ref bucket, noiseMap, x, y + 1, clamp);
            }
        }
        
        if (y != 0)
        {
            if (IsValidPoint(ref checkedPoints, noiseMap, x, y - 1, clamp))
            {
                bucket.Add(new Vector2(x, y - 1));
                CheckNeighbourPoints(ref checkedPoints, ref bucket, noiseMap, x, y - 1, clamp);
            }
        }
    }

    private bool IsValidPoint(ref bool[][] checkedPoints, float[,] noiseMap, int x, int y, float clamp)
    {
        if (checkedPoints[x][y]) return false;
        checkedPoints[x][y] = true;
        
        return noiseMap[x, y] > clamp;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
