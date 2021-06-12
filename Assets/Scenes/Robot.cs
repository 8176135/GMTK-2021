using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private MainBlock mainBlock;
    private Rigidbody2D rigidbody2D;

    // private PidController pid;
    
    public float robotMaxTorque = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        // pid = new PidController(1.0, 1.0, )
    }

    void Thrust(Vector2 thrustValue)
    {
        var thrusters = mainBlock.thrusters;
    }

    void Turning(Vector2 targetDirection)
    {
        var thrusters = mainBlock.thrusters;
        var facingDirection = this.rigidbody2D.transform.up;
        var difAngle = Vector2.SignedAngle(facingDirection, targetDirection);
        Debug.Log(difAngle);
        rigidbody2D.AddTorque(Mathf.Clamp(difAngle * 0.05f, -1.0f, 1.0f) * robotMaxTorque * Time.deltaTime);
        
    }


    // Update is called once per frame
    void Update()
    {
        Turning(Vector2.left);
    }
}