using UnityEngine;

public class StandardCannon : Weapon
{
    public float spawnInterval = 10;

    public float spawnCooldown = 0;

    public bool fireWeapon = false;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown < spawnInterval)
        {
            spawnCooldown += Time.deltaTime;
            while (spawnCooldown > spawnInterval)
            {
                spawnCooldown -= spawnInterval;
            }
        }
    }

    protected override void FireWeapon()
    {
        fireWeapon = true;
    }

    protected override void StopFiringWeapon()
    {
        fireWeapon = false;
    }
}