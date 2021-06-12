using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected MainBlock mainBlock;
    public Vector3? target = null;
    public bool fireWeapon = false;

    public void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        mainBlock.connectedToParent.AddListener(c => c.NewWeapon(this));
    }
    public abstract void StartFiringWeapon();
    public abstract void StopFiringWeapon();
}
