using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected MainBlock mainBlock;
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        mainBlock.connectedToParent.AddListener(c => c.NewWeapon(this));
    }
    protected abstract void FireWeapon();
    protected abstract void StopFiringWeapon();
}
