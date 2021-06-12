using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initVelocity;
    private new Rigidbody2D rigidbody2D;
    public MainBlock ownedRootBlock;

    public float timeToLive = 10;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        rigidbody2D.AddRelativeForce(Vector2.up * initVelocity, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Expiration time on bullet
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
        {
            Destroy(gameObject);
        }
    }
}
