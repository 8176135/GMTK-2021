using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiController : MonoBehaviour
{
    private int counter = 0;
    public int numberOfParts = 15;
    public float spawnSpeed = 0.15f;
    public float combineForce = 1.5f;

    public GameObject[] partsList;

    public PlayerController player;

    public Robot robot;
    public MainBlock mainBlock;

    public float awarenessRadius = 100.0f;
    public float searchRadius = 50.0f;

    public MainBlock currentNearbyLooseBlock;
    public bool hasNearbyLooseBlock = false;

    public bool NotFromSpawner = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        robot = GetComponent<Robot>();
        mainBlock = GetComponent<MainBlock>();
        InvokeRepeating(nameof(FindNearbyLoosePart), 1.0f, 1.0f);
        if (NotFromSpawner)
        {
            StartSpawnSequence(1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsDestroyed())
        {
            var blockDiff = this.mainBlock.BlockCount - player.robot.mainBlock.BlockCount;
            var posDiff = player.transform.position - transform.position;
            if (posDiff.magnitude < awarenessRadius && counter >= numberOfParts)
            {
                if (blockDiff < -1) // Has less blocks than player
                {
                    if (hasNearbyLooseBlock && !currentNearbyLooseBlock.IsDestroyed() &&
                        !currentNearbyLooseBlock.connectedToShip)
                    {
                        var posDiff2 = currentNearbyLooseBlock.transform.position - transform.position;
                        robot.Thrust(posDiff2.normalized);
                    }
                }
                else // Has more blocks than player
                {
                    if (posDiff.magnitude > 3.0)
                    {
                        robot.Thrust(posDiff.normalized);
                    }
                    else if (posDiff.magnitude < 0.5f)
                    {
                        robot.Thrust(-posDiff.normalized);
                    }
                }

                robot.Fire(true);
                robot.SetAimTarget(player.transform.position);
            }
        }
    }

    void FindNearbyLoosePart()
    {
        var position = transform.position;
        var results = Physics2D.OverlapCircleAll(position, searchRadius, LayerMask.GetMask("Default"));
        // Debug.Log(results);
        var others = results
            .OrderBy(c => (c.transform.position - position).sqrMagnitude)
            .Select(c => c.GetComponent<MainBlock>())
            .FirstOrDefault(c => c != null && !c.connectedToShip);
        hasNearbyLooseBlock = others != null;

        this.currentNearbyLooseBlock = others;
    }

    public void StartSpawnSequence(int numberOfParts)
    {
        this.numberOfParts = Random.Range(Math.Max(0, numberOfParts - 1), numberOfParts + 1);
        InvokeRepeating(nameof(SpawnSequence), spawnSpeed, spawnSpeed);
    }

    void SpawnSequence()
    {
        if (counter >= numberOfParts)
        {
            CancelInvoke(nameof(SpawnSequence));
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            return;
        }

        var idx = Random.Range(0, partsList.Length);
        
        var attempts = 3;
        while (attempts-- > 0)
        {
            var offset = (Random.insideUnitCircle.normalized * (1 + counter * 0.1f));
            var spawnPos = offset + (Vector2) this.transform.position;
            
            if (Physics2D.OverlapCircle(spawnPos, 0.25f, LayerMask.GetMask("WorldObstacle")) == null)
            {
                var a = Instantiate(partsList[idx],
                    offset + (Vector2) this.transform.position,
                    Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360)));

                // Debug.DrawLine((Vector2) this.transform.position, (Random.insideUnitCircle.normalized * 2) + (Vector2) this.transform.position, Color.red, 100.0f);

                a.GetComponent<Rigidbody2D>().AddForce(-offset * combineForce, ForceMode2D.Impulse);

                counter++;
            }
        }


    }
}