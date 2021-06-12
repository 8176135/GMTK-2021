using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    private int counter = 0;
    public int numberOfParts = 10;
    public float spawnSpeed = 0.25f;

    public GameObject[] partsList;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnSequence), spawnSpeed, spawnSpeed);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnSequence()
    {
        if (counter > numberOfParts)
        {
            CancelInvoke(nameof(SpawnSequence));
            return;
        }

        var idx = Random.Range(0, partsList.Length);

        var offset = (Random.insideUnitCircle.normalized * (2 + counter * 0.25f));

        var a = Instantiate(partsList[idx],
            offset + (Vector2) this.transform.position,
            Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360)));
        
        // Debug.DrawLine((Vector2) this.transform.position, (Random.insideUnitCircle.normalized * 2) + (Vector2) this.transform.position, Color.red, 100.0f);

        a.GetComponent<Rigidbody2D>().AddForce(-offset * 1, ForceMode2D.Impulse);

        counter++;
    }
}