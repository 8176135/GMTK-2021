using MilkShake;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class StandardCannon : Weapon
{
    public float weaponInterval = 0.5f;
    public float weaponCooldown = 0;

    public GameObject bullet;
    public Transform rendererTransform;

    // public Animator animator;
    private static readonly int IsShooting = Animator.StringToHash("IsShooting");

    public ShakePreset shakerPreset;

    private ParticleSystem muzzleFlash;
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        weaponInterval += Random.Range(weaponInterval * -0.2f, weaponInterval * 0.2f);
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
        if (weaponCooldown >= weaponInterval)
        {
            // Please keep this as -= not set to 0
            weaponCooldown -= weaponInterval;
            var position = transform.position;
            var spawnedObj = Instantiate(bullet, position, rendererTransform.rotation);
            var spawnedBullet = spawnedObj.GetComponent<Bullet>();
            this.mainBlock.rigidbody.AddForce(-transform.forward * 10, ForceMode2D.Impulse);
            spawnedBullet.ownedRootBlock = this.mainBlock.GetRootBlock();
            Shaker.ShakeAllFromPoint(position + new Vector3(0,0,-10), 10.0f, shakerPreset);
            // muzzleFlash.transform.rotation = transform.rotation;
            muzzleFlash.Clear();
            muzzleFlash.Play();
            _audioSource.Play();
        }
    }

    public override void StartFiringWeapon()
    {
        // animator.SetBool(IsShooting, true);
        fireWeapon = true;
    }

    public override void StopFiringWeapon()
    {
        // animator.SetBool(IsShooting, false);
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
