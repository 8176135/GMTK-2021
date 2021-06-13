using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Main controller of a ship. This class is the overseer of all blocks attached to a ship.
/// </summary>
public class Robot : MonoBehaviour
{
    public MainBlock mainBlock;
    private new Rigidbody2D rigidbody2D;
    /// <summary>
    /// Where the player is looking (for player, it's where the mouse is)
    /// </summary>
    public Vector2 targetPosition; 
    /// <summary>
    /// Where the player is going (for player, it's determined by WASD)
    /// </summary>
    private Vector2 controlDirection; 

    public float turnSpeedPerThruster = 400.0f;
    public float thrustPowerSpeedPerThruster = 1.0f;
    
    // private PidController pid;

    public float robotMaxTorque = 10.0f;

    public AudioClip audioDeath;

    // Start is called before the first frame update

    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        // pid = new PidController(1.0, 1.0, )
    }

    /// <summary>
    /// Callback function for when a player/AI wishes to fire a weapon.
    /// </summary>
    /// <param name="pressed">True if this is a press action. False if this is release action.</param>
    public void Fire(bool pressed)
    {
        var weapons = mainBlock.weapons;

        foreach (var weapon in weapons)
        {
            if (pressed)
            {
                weapon.Value.StartFiringWeapon();
            }
            else
            {
                weapon.Value.StopFiringWeapon();
            }
        }
    }

    /// <summary>
    /// Callback function for when a player/AI wishes to thrust in a direction.
    /// </summary>
    /// <param name="normalisedThrustTarget">Control direction of thrust. Should be normalised.</param>
    public void Thrust(Vector2 normalisedThrustTarget)
    {
        controlDirection = normalisedThrustTarget;
        // UpdateThrusters();
    }

    /// <summary>
    /// Callback function for turning to a specific position.
    /// </summary>
    /// <param name="targetPosition"></param>
    public void Turning(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    /// <summary>
    /// Visual update for thrusters.
    /// </summary>
    public void UpdateThrusters()
    {
        var thrusters = mainBlock.thrusters;

        var force = controlDirection.magnitude;
        var rotation = Vector2.SignedAngle(Vector2.up, controlDirection);

        foreach (var thruster in thrusters)
        {
            thruster.Value.SetVisuals(rotation, force);
        }
    }


    private List<GameObject> redundentObjs = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        var thrusters = mainBlock.thrusters;
        var facingDirection = this.rigidbody2D.transform.up;

        var position = transform.position;
        var difAngle = Vector2.SignedAngle(facingDirection, (Vector2) position - targetPosition);
        var actualTorque = robotMaxTorque + thrusters.Count * turnSpeedPerThruster; 
        rigidbody2D.AddTorque(Mathf.Clamp(difAngle * 0.05f, -1.0f, 1.0f) * actualTorque * Time.deltaTime);
        
        foreach (var thruster in thrusters)
        {
            // TODO: Figure out why this is needed
            if (!thruster.Value.IsDestroyed())
            {
                thruster.Value.Rigidbody2D.AddForce(controlDirection * thrustPowerSpeedPerThruster);
            }
            else
            {
                redundentObjs.Add(thruster.Key);
            }
        }

        foreach (var obj in redundentObjs)
        {
            thrusters.Remove(obj);
        }

        redundentObjs.Clear();

        rigidbody2D.AddForce(controlDirection * 1f);
        
        UpdateThrusters();
    }

    private void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(audioDeath, transform.position);
    }

    public void SetAimTarget(Vector2 aimPos)
    {
        foreach (var mainBlockWeapon in mainBlock.weapons.Values)
        {
            mainBlockWeapon.target = aimPos;
        }
    }
}
