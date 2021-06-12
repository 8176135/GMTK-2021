using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MainBlock : MonoBehaviour
{
    private HashSet<MainBlock> connectedObjects = new HashSet<MainBlock>();
    private MainBlock parentBlock;
    private FixedJoint2D parentJoint;
    private new Rigidbody2D rigidbody;

    public Dictionary<GameObject, Thruster> thrusters = new Dictionary<GameObject, Thruster>();
    public Dictionary<GameObject, Weapon> weapons = new Dictionary<GameObject, Weapon>();

    public UnityEvent<MainBlock> connectedToParent;

    public float massSum;
    public Vector2 calculatedCenterOfMass;

    // public float debugThrustValue = 50;

    public bool connectedToShip = false;

    // Start is called before the first frame update
    void Start()
    {
        parentJoint = this.GetComponent<FixedJoint2D>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        massSum = rigidbody.mass;
        calculatedCenterOfMass = rigidbody.centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.SetPositionAndRotation();
    }

    public void NewThruster(Thruster newBlock)
    {
        this.thrusters.Add(newBlock.gameObject, newBlock);
        if (this.parentBlock != false)
        {
            this.parentBlock.NewThruster(newBlock);
        }
    }

    public void NewWeapon(Weapon newBlock)
    {
        this.weapons.Add(newBlock.gameObject, newBlock);
        if (this.parentBlock != false)
        {
            this.parentBlock.NewWeapon(newBlock);
        }
    }

    public void RemoveMisc(GameObject newBlock)
    {
        this.thrusters.Remove(newBlock);
        if (this.parentBlock != false)
        {
            this.parentBlock.RemoveMisc(newBlock);
        }
    }

    void RemoveFromParent()
    {
        if (this.parentBlock != false)
        {
            this.parentBlock.connectedObjects.Remove(this);
        }

        RemoveMisc(this.gameObject);
    }

    void ConnectToShip(MainBlock otherBlock)
    {
        this.connectedToShip = true;
        this.parentBlock = otherBlock;
        parentJoint.connectedBody = this.parentBlock.GetComponent<Rigidbody2D>();
        parentJoint.enabled = true;
        otherBlock.connectedObjects.Add(this);
        otherBlock.massSum = otherBlock.GetMassSum();
        otherBlock.UpdateCenterOfMass();
        connectedToParent.Invoke(otherBlock);
    }

    float GetMassSum()
    {
        return this.connectedObjects.Aggregate(this.rigidbody.mass, (a, b) => a + b.GetMassSum());
    }

    Vector2 UpdateCenterOfMass()
    {
        var position = transform.position;
        var answer = connectedObjects.Aggregate((baseCenterOfMass: (Vector2) position, rigidbody.mass), (a, b) =>
        {
            a.baseCenterOfMass += (b.GetCenterOfMass() + (Vector2)b.transform.position) * b.massSum;
            a.mass += b.massSum;
            return a;
            // var (vector2, mass) = a;
            // var massSum2 = (b.massSum + mass);
            //
            // var relativePosition = (Vector2)b.transform.position - vector2;
            // return (Vector2.Lerp(b.rigidbody.centerOfMass + (Vector2)relativePosition, vector2, mass / massSum2), massSum2);
        });

        this.calculatedCenterOfMass = ((answer.baseCenterOfMass / answer.mass) - (Vector2) position);
        if (this.parentBlock != null)
        {
            parentBlock.UpdateCenterOfMass();
        }

        return GetCenterOfMass();
    }

    public Vector2 GetCenterOfMass()
    {
        return Quaternion.AngleAxis(transform.localRotation.eulerAngles.z, Vector3.forward) * calculatedCenterOfMass;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        var block = collision.gameObject.GetComponent<MainBlock>();

        if (block != null)
        {
            if (!block.connectedToShip && this.connectedToShip)
            {
                block.ConnectToShip(this);
            }
            else
            {
            }
        }
        else
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                RemoveFromParent();
                Destroy(gameObject);
                Destroy(bullet.gameObject);
                // TODO: spawn explosion
            }
        }
    }
}