using Unity.VisualScripting;
using UnityEngine;

public class StandardCannon : Weapon
{
    public float weaponInterval = 1;
    public float weaponCooldown = 0;

    public GameObject bullet;
    public Transform rendererTransform;


    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.HasValue)
        {
            var actualRotation = Vector2.SignedAngle(Vector2.up, target.Value - transform.position);
            rendererTransform.rotation =
                Quaternion.RotateTowards(rendererTransform.rotation, Quaternion.Euler(0, 0, actualRotation), 4f);
            if (weaponCooldown < weaponInterval)
            {
                weaponCooldown += Time.deltaTime;
            }

            if (fireWeapon)
            {
                FireWeapon();
            }
        }
    }

    private Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform closestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistance = Mathf.Infinity;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                closestTarget = potentialTarget;
            }
        }

        return closestTarget;
    }

    private void FireWeapon()
    {
        while (weaponCooldown > weaponInterval)
        {
            weaponCooldown -= weaponInterval;
            var spawnedObj = Instantiate(bullet, transform.position, rendererTransform.rotation);
            var spawnedBullet = spawnedObj.GetComponent<Bullet>();
            this.mainBlock.rigidbody.AddForce(-transform.forward * 10, ForceMode2D.Impulse);
            spawnedBullet.ownedRootBlock = this.mainBlock.GetRootBlock();
        }
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

        var position = transform.position;
        var rotation = Vector2.SignedAngle(Vector2.up, target - new Vector2(position.x, position.y));
        rendererTransform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
