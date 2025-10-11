using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    public float bulletSpeed;
    float fireRate;

    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))    // Press Z to shoot
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
