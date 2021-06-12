using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected abstract void FireWeapon();
    protected abstract void StopFiringWeapon();
}
