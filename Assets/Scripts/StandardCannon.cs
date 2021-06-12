using Unity.VisualScripting;
using UnityEngine;

public class StandardCannon : Weapon
{
    public float spawnInterval = 10;
    public float spawnCooldown = 0;
    public bool fireWeapon = false;

    public GameObject bullet;
    public Transform rendererTransform;

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown < spawnInterval)
        {
            spawnCooldown += Time.deltaTime;
            if (spawnCooldown > spawnInterval)
            {
                FireWeapon();
                spawnCooldown = 0;
            }
        }
        
    }

    private Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform closestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistance = Mathf.Infinity;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                closestTarget = potentialTarget;
            }
        }
        
        return closestTarget;
    }

    private void FireWeapon()
    {
        Instantiate(bullet, transform.position, rendererTransform.rotation);
    }

    public override void StartFiringWeapon()
    {
        fireWeapon = true;
    }

    public override void StopFiringWeapon()
    {
        fireWeapon = false;
    }

    public void Aim(Vector2 target)
    {
        if (!rendererTransform) return;

        var rotation = Vector2.SignedAngle(Vector2.up, target - new Vector2(transform.position.x, transform.position.y));
        rendererTransform.rotation = Quaternion.Euler(0,0,rotation);
    }
}