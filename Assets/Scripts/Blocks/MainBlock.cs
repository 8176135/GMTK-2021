using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkShake;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class MainBlock : MonoBehaviour
{
    public HashSet<MainBlock> connectedObjects = new HashSet<MainBlock>();
    public int BlockCount = 1;
    private MainBlock parentBlock;
    private FixedJoint2D parentJoint;
    public new Rigidbody2D rigidbody;

    public Dictionary<GameObject, Thruster> thrusters = new Dictionary<GameObject, Thruster>();
    public Dictionary<GameObject, Weapon> weapons = new Dictionary<GameObject, Weapon>();

    public UnityEvent<MainBlock> connectedToParent;

    // public float massSum;
    public Vector2 calculatedCenterOfMass;

    public ShakePreset destroyedShakePreset;

    // public float debugThrustValue = 50;

    public bool connectedToShip = false;
    
    // Audio stuff
    private AudioSource audioSource;
    public AudioClip audioAttach;
    public AudioClip audioDetach;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        parentJoint = this.GetComponent<FixedJoint2D>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        // massSum = rigidbody.mass;
        calculatedCenterOfMass = rigidbody.centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.SetPositionAndRotation();
    }

    public MainBlock GetRootBlock()
    {
        if (!this.connectedToShip)
        {
            return this;
        }

        if (this.parentBlock)
        {
            return this.parentBlock.GetRootBlock();
        }
        else
        {
            return this;
        }
    }

    public int GetDifficulty()
    {
        return thrusters.Count + weapons.Count;
    }

    public void NewThruster(Thruster newBlock)
    {
        this.thrusters.Add(newBlock.gameObject, newBlock);
        if (this.parentBlock)
        {
            this.parentBlock.NewThruster(newBlock);
        }
    }

    public void NewWeapon(Weapon newBlock)
    {
        this.weapons.Add(newBlock.gameObject, newBlock);
        if (this.parentBlock)
        {
            this.parentBlock.NewWeapon(newBlock);
        }
    }

    public void RemoveMisc(GameObject newBlock)
    {
        if (this.thrusters.Remove(newBlock))
        {
            (newBlock.GetComponent<Thruster>()).SetVisuals(0, 0);
        }

        ;
        if (this.weapons.Remove(newBlock))
        {
            (newBlock.GetComponent<Weapon>()).StopFiringWeapon();
        }

        if (this.parentBlock)
        {
            this.parentBlock.RemoveMisc(newBlock);
        }
    }

    void RemoveFromParent()
    {
        var toRemoveList = connectedObjects.ToList();
        foreach (var connectedObject in toRemoveList)
        {
            connectedObject.RemoveFromParent();
        }

        if (this.parentBlock && !this.parentBlock.IsDestroyed() && !this.gameObject.IsDestroyed())
        {
            this.parentBlock.RemoveMisc(this.gameObject);

            this.parentBlock.connectedObjects.Remove(this);
            this.parentBlock.UpdateBlockCount(-BlockCount);
        }

        this.parentJoint.enabled = false;
        this.parentJoint.connectedBody = null;
        this.parentBlock = null;
        this.connectedToShip = false;
        audioSource.PlayOneShot(audioDetach);
    }

    void ConnectToShip(MainBlock otherBlock)
    {
        this.connectedToShip = true;
        this.parentBlock = otherBlock;
        parentJoint.connectedBody = this.parentBlock.GetComponent<Rigidbody2D>();
        parentJoint.enabled = true;
        otherBlock.connectedObjects.Add(this);
        // otherBlock.massSum = otherBlock.GetMassSum();
        otherBlock.UpdateBlockCount(this.BlockCount);
        // otherBlock.UpdateCenterOfMass();
        connectedToParent.Invoke(otherBlock);
        audioSource.PlayOneShot(audioAttach);
    }

    void UpdateBlockCount(int delta)
    {
        BlockCount += delta;
        if (this.parentBlock)
        {
            this.parentBlock.UpdateBlockCount(delta);
        }
    }

    float GetMassSum()
    {
        return this.connectedObjects.Aggregate(this.rigidbody.mass, (a, b) => a + b.GetMassSum());
    }

    // Vector2 UpdateCenterOfMass()
    // {
    //     // var position = transform.position;
    //     // var answer = connectedObjects.Aggregate((baseCenterOfMass: (Vector2) position, rigidbody.mass), (a, b) =>
    //     // {
    //     //     a.baseCenterOfMass += (b.rigidbody.worldCenterOfMass) * b.massSum;
    //     //     a.mass += b.massSum;
    //     //     return a;
    //     //     // var (vector2, mass) = a;
    //     //     // var massSum2 = (b.massSum + mass);
    //     //     //
    //     //     // var relativePosition = (Vector2)b.transform.position - vector2;
    //     //     // return (Vector2.Lerp(b.rigidbody.centerOfMass + (Vector2)relativePosition, vector2, mass / massSum2), massSum2);
    //     // });
    //     //
    //     // this.rigidbody.centerOfMass = Quaternion.AngleAxis(-transform.localRotation.eulerAngles.z, Vector3.forward) *
    //     //                               ((answer.baseCenterOfMass / answer.mass) - (Vector2) position);
    //     // if (this.parentBlock != null)
    //     // {
    //     //     parentBlock.UpdateCenterOfMass();
    //     // }
    //     //
    //     // return GetCenterOfMass();
    // }

    public Vector2 GetCenterOfMass()
    {
        return rigidbody.centerOfMass;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var block = collision.gameObject.GetComponent<MainBlock>();

        if (block != null)
        {
            if (this.connectedToShip)
            {
                if (!block.connectedToShip)
                {
                    block.ConnectToShip(this);
                }
                // else if (collision.relativeVelocity.magnitude > 1.0f)
                // {
                //     block.RemoveFromParent();
                //     this.RemoveFromParent();
                //     Destroy(block.gameObject);
                //     Destroy(gameObject);
                //     var hitPoint = collision.GetContact(0).point;
                //     var toPush = Physics2D.OverlapCircleAll(hitPoint, 4.0f, ~LayerMask.GetMask("WorldObstacle"));
                //     foreach (var stuff in toPush)
                //     {
                //         if (stuff.GetComponent<MainBlock>() != null)
                //         {
                //             stuff.attachedRigidbody.AddForceAtPosition(
                //                 (hitPoint - stuff.attachedRigidbody.position).normalized * 4, hitPoint, ForceMode2D.Impulse);
                //         }
                //     }
                // }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (!bullet.IsDestroyed() && bullet.ownedRootBlock.GetInstanceID() != GetRootBlock().GetInstanceID())
            {
                RemoveFromParent();
                Destroy(bullet.transform.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Shaker.ShakeAllFromPoint(transform.position + new Vector3(0, 0, -10.0f), 15.0f, destroyedShakePreset);
    }
}
