using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    public float bulletSpeed;
    float fireRate;

    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;
    public PlayerControl pc;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && pc.crashed == 0)    // Press Z to shoot if not crashed
        {
            Shoot();
        }
    }

    void Shoot()    // spawns in a bullet and gives it momentum
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward*bulletSpeed, ForceMode.Impulse);
    }
}
