using UnityEngine;

public class spawnItems : MonoBehaviour
{
    public float spawnTime;
    float timer = 0;
    public GameObject tempItem; // temporary useless item for early testing
    float randX, randY, randZ;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTime)
        {
            randX = (Random.value * 500) - 250; // random x coordinate from -250 to +250
            randZ = (Random.value * 500) - 250; // random z coordinate from -250 to +250
            randY = (Random.value * 90) + 10; // random y coordinate from 10 to 100
            Vector3 randSpawn = new Vector3 (randX, randY, randZ);
            GameObject Item = Instantiate(tempItem, randSpawn, Quaternion.identity);    // Spawns item
            timer -= spawnTime;
        }
    }
}
