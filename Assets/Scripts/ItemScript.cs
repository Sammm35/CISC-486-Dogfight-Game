using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

public class ItemScript : MonoBehaviour
{
    public float lifeTime;
    public livesController lives;
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
        if (layerNumber == 6) { lives.itemCollected(0); }
        if (layerNumber == 8) { lives.itemCollected(1); }
        Destroy(gameObject);
    }
}
