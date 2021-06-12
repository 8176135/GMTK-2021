using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private MainBlock mainBlock;
    private new Rigidbody2D rigidbody2D;
    public Vector2 targetDirection;
    public Vector2 thrustForce;
    private Vector2 controlDirection;

    public GameObject dummy;

    // private PidController pid;

    public float robotMaxTorque = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        // pid = new PidController(1.0, 1.0, )
    }

    public void Thrust(Vector2 normalisedThrustTarget)
    {
        var thrusters = mainBlock.thrusters;

        var thrusterCount = 0;
        thrusterCount = thrusters.Count;

        var multiplier = (thrusterCount + 0.5f) * 500;
        controlDirection = normalisedThrustTarget;
        thrustForce = normalisedThrustTarget * (multiplier);
        
        UpdateThrusters();
    }

    public void Turning(Vector2 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void UpdateThrusters()
    {
        var thrusters = mainBlock.thrusters;
        if (thrusters == null) return;

        var force = controlDirection.magnitude;
        var rotation = Vector2.SignedAngle(Vector2.up, thrustForce);
        
        foreach (var thruster in thrusters)
        {
            thruster.Value.setVisuals(rotation, force);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        var thrusters = mainBlock.thrusters;
        var facingDirection = this.rigidbody2D.transform.up;

        var position = transform.position;
        var difAngle = Vector2.SignedAngle(facingDirection, (Vector2) position - targetDirection);
        var actualTorque = robotMaxTorque + thrusters.Count * 20; 
        // rigidbody2D.AddTorque(Mathf.Clamp(difAngle * 0.05f, -1.0f, 1.0f) * actualTorque * Time.deltaTime);

        rigidbody2D.AddForceAtPosition(thrustForce * Time.deltaTime, mainBlock.GetCenterOfMass() + (Vector2)position);
        dummy.transform.SetPositionAndRotation(mainBlock.GetCenterOfMass() + (Vector2)position, Quaternion.identity);
        UpdateThrusters();
    }
}
