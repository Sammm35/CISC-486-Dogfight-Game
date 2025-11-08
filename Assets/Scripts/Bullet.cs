using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public livesController lives;
    public float damage;
    public float lifeTime = 3;

    private void Start()
    {
        if (lives == null)
            lives = FindFirstObjectByType<livesController>();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int layerNumber = other.gameObject.layer;
        // Enemy is layer 8, player is layer 6
        if (layerNumber == 6) { lives.hit(0); }
        if (layerNumber == 8) { lives.hit(1); }

        Destroy(gameObject);
    }

}
