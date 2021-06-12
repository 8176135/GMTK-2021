using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int mapWidth = 64;
    public int mapHeight = 64;
    public int noiseScale = 4;
    [Range(0, 1)]
    public float noiseClamp = 0f;
    [Range(1, 28)]
    public int noiseOctaves = 4;
    [Range(0, 1)]
    public float noisePersistance = 0.5f;
    public float noiseLacunarity = 2f;
    public int Seed = 0;
    public Vector2 Offset = Vector2.zero;

    public bool autoUpdate = true;

    public float[,] noiseMap;

    public void GenerateMap()
    {
        mapWidth = mapWidth <= 0 ? 1 : mapWidth;
        mapHeight = mapHeight <= 0 ? 1 : mapHeight;
        noiseScale = noiseScale <= 0 ? 1 : noiseScale;

        Noise.Scale = noiseScale;
        Noise.Clamp = noiseClamp;
        Noise.Octaves = noiseOctaves;
        Noise.Persistance = noisePersistance;
        Noise.Lacunarity = noiseLacunarity;
        Noise.Seed = Seed;
        Noise.Offset = Offset;
        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight);

        var display = GetComponent<MapDisplay>();
        display.DrawNoiseMap(noiseMap, noiseClamp);
    }
    
    private void OnValidate()
    {
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        
        if (noiseLacunarity < 1) noiseLacunarity = 1;
        if (noiseOctaves < 0) noiseOctaves = 0;
    }
}
