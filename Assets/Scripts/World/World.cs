using UnityEngine;

public class World : MonoBehaviour
{
    [Header("World Limits")]
    public Transform wallNorth;
    public Transform wallSouth;
    public Transform wallEast;
    public Transform wallWest;
    public int size = 2048;
    
    [Header("WorldSeed")]
    public bool useRandomSeed;
    public int seed;

    public bool autoUpdate;
    
    
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

        var halfSize = (float) size / 2;
        
        wallNorth.position = new Vector3(0f, halfSize);
        wallSouth.position = new Vector3(0f, -halfSize);
        wallEast.position = new Vector3(halfSize, 0f);
        wallWest.position = new Vector3(-halfSize, 0f);

        wallNorth.localScale = new Vector3(size, 64f, 1f);
        wallSouth.localScale = new Vector3(size, 64f, 1f);
        wallEast.localScale = new Vector3(size, 64f, 1f);
        wallWest.localScale = new Vector3(size, 64f, 1f);
    }
}
