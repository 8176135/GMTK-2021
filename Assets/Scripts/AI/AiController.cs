using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject player;

    public Robot robot;
    public MainBlock mainBlock;

    public float awarenessRadius = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        robot = GetComponent<Robot>();
        mainBlock = GetComponent<MainBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsDestroyed())
        {
            var posDiff = player.transform.position - transform.position;
            if (posDiff.magnitude < awarenessRadius && counter > numberOfParts)
            {
                robot.Thrust(posDiff.normalized);
                robot.Fire(true);
                robot.SetAimTarget(player.transform.position);
            }
        }

    }

    public void StartSpawnSequence(int numberOfParts)
    {
        this.numberOfParts = Random.Range(Math.Max(0, numberOfParts - 1), numberOfParts + 1);
        InvokeRepeating(nameof(SpawnSequence), spawnSpeed, spawnSpeed);
    }

    void SpawnSequence()
    {
        if (counter > numberOfParts)
        {
            CancelInvoke(nameof(SpawnSequence));
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            return;
        }

        var idx = Random.Range(0, partsList.Length);

        var offset = (Random.insideUnitCircle.normalized * (1 + counter * 0.1f));

        var a = Instantiate(partsList[idx],
            offset + (Vector2) this.transform.position,
            Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360)));

        // Debug.DrawLine((Vector2) this.transform.position, (Random.insideUnitCircle.normalized * 2) + (Vector2) this.transform.position, Color.red, 100.0f);

        a.GetComponent<Rigidbody2D>().AddForce(-offset * combineForce, ForceMode2D.Impulse);

        counter++;
    }
}