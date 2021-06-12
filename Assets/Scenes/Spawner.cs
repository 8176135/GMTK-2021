using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 10;
    
    
    private float spawnCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnCooldown += Time.deltaTime;
        while (spawnCooldown > spawnInterval)
        {
            spawnCooldown -= spawnInterval;
            SpawnStuff();
        }
    }

    void SpawnStuff()
    {
        // Instantiate()
    }
}
