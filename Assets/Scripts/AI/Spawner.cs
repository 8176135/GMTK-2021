using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 10;
    public GameObject[] spawnList;
    public GameObject player;
    private Transform playerTransform;
    private MainBlock playerMainBlock;
    
    
    private float spawnCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        playerMainBlock = player.GetComponent<MainBlock>();
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
        var idx = Random.Range(0, spawnList.Length);
        
        var attempts = 5;
        while (attempts-- > 0)
        {
            var offset = Random.insideUnitCircle.normalized * (playerMainBlock.BlockCount * 0.5f + 10 + Random.Range(-3.0f, 3.0f));
            var spawnPos = offset + (Vector2) playerTransform.position;
            if (Physics2D.OverlapCircle(spawnPos, 3.0f, LayerMask.GetMask("WorldObstacle")) == null)
            {
                var newObject = Instantiate(
                    spawnList[idx],
                    offset + (Vector2)playerTransform.position,
                    Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360))
                );

                var aiController = newObject.GetComponent<AiController>();
                if (aiController != null)
                {
                    var difficulty = playerMainBlock.GetDifficulty();
                    aiController.StartSpawnSequence(difficulty);
                }
                
                return;
            };
        }
        
        Debug.Log("All 5 spawn attempts failed");
    }
}
