using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MainBlock : MonoBehaviour
{
    private List<MainBlock> connectedObjects = new List<MainBlock>();
    private MainBlock parentBlock;
    private FixedJoint2D parentJoint;
    private Rigidbody2D rigidbody;

    public Dictionary<GameObject, Thruster> thrusters;
    public Dictionary<GameObject, Weapon> weapons;

    public UnityEvent<MainBlock> connectedToParent;

    // public float debugThrustValue = 50;

    public bool connectedToShip = false;

    // Start is called before the first frame update
    void Start()
    {
        parentJoint = this.GetComponent<FixedJoint2D>();
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewThruster(Thruster newBlock)
    {
        this.thrusters.Add(newBlock.gameObject, newBlock);
        this.parentBlock.NewThruster(newBlock);
    }
    
    public void NewWeapon(Weapon newBlock)
    {
        this.weapons.Add(newBlock.gameObject, newBlock);
        this.parentBlock.NewWeapon(newBlock);
    }

    void ConnectToShip(MainBlock otherBlock)
    {
        this.connectedToShip = true;
        this.parentBlock = otherBlock;
        parentJoint.connectedBody = this.parentBlock.GetComponent<Rigidbody2D>();
        parentJoint.enabled = true;
        otherBlock.connectedObjects.Add(this);

        connectedToParent.Invoke(otherBlock);
    }

    float GetMassSum()
    {
        var sum = this.connectedObjects.Aggregate(0.0f, (a, b) => a + b.GetMassSum());
        return sum + this.rigidbody.mass;
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
    }
}