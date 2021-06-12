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

    private void FireWeapon()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    public override void StartFiringWeapon()
    {
        fireWeapon = true;
    }

    public override void StopFiringWeapon()
    {
        fireWeapon = false;
    }

    public override void Aim(Vector2 target)
    {
        if (!rendererTransform) return;

        var rotation = Vector2.SignedAngle(Vector2.up, target - new Vector2(transform.localPosition.x, transform.localPosition.y));
        rendererTransform.localRotation = Quaternion.RotateTowards(rendererTransform.localRotation, Quaternion.Euler(0,0,rotation), 4f);
    }
}