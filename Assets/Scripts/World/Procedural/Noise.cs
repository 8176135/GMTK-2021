using UnityEngine;

public static class Noise
{
    public static float Scale = 20f;
    public static float Clamp = 0.0f;
    public static int Octaves = 3;
    public static float Persistance = 0.5f;
    public static float Lacunarity = 2f;
    public static Vector2 Offset = Vector2.zero;
    public static int Seed = 0;

    private static Vector2[] CreateOctaveOffsets(System.Random prng)
    {
        var octaveOffsets = new Vector2[Octaves];
        for (int i = 0; i < Octaves; i++)
        {
            var offsetX = prng.Next(-10000, 100000) + Offset.x;
            var offsetY = prng.Next(-10000, 100000) + Offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        return octaveOffsets;
    }
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight)
    {
        Scale = Scale <= 0f ? 0.0001f : Scale;

        var prng = new System.Random(Seed);
        var octaveOffsets = CreateOctaveOffsets(prng);
        var noiseMap = new float[mapWidth, mapHeight];

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        // Zoom to middle of noise instead of top right corner
        var halfWidth = mapWidth / 2;
        var halfHeight = mapHeight / 2;
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var amplitude = 1f;
                var frequency = 1f;
                var noiseHeight = 0f;
                
                for (int i = 0; i < Octaves; i++) {
                    var xSample = (x - halfWidth) / Scale * frequency + octaveOffsets[i].x;
                    var ySample = (y - halfHeight) / Scale * frequency + octaveOffsets[i].y;

                    // Offset the 0 - 1 value to -0.5 - 0.5
                    // This prevents overflowing the map with only high values
                    var perlinValue = Mathf.PerlinNoise(xSample, ySample) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= Persistance;
                    frequency *= Lacunarity;
                }

                maxNoiseHeight = noiseHeight > maxNoiseHeight ? noiseHeight : maxNoiseHeight;
                minNoiseHeight = noiseHeight < minNoiseHeight ? noiseHeight : minNoiseHeight;
                
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize noise map
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var value = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

                noiseMap[x, y] = Clamp == 0f 
                    ? value 
                    : (value > Clamp ? 1f : 0f);
            }
        }

        return noiseMap;
    }
}
