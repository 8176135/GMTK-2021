using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private MainBlock mainBlock;
    private new Rigidbody2D rigidbody2D;
    public Vector2 targetDirection;
    public Vector2 thrustForce;
    private Vector2 controlDirection;

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
        if (thrusters != null)
        {
            thrusterCount = thrusters.Count;
        }
        var multiplier = thrusterCount + 1000;

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
        var difAngle = Vector2.SignedAngle(facingDirection, targetDirection);
        rigidbody2D.AddTorque(Mathf.Clamp(difAngle * 0.05f, -1.0f, 1.0f) * robotMaxTorque * Time.deltaTime);
        
        rigidbody2D.AddForce(thrustForce * Time.deltaTime);

        UpdateThrusters();
    }
}
