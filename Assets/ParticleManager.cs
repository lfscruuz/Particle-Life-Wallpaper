using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject particlePrefab;
    public static int numTypes = 8;
    public int colorStep = Mathf.RoundToInt((float) 360/numTypes);
    public int particleCount = 20000;
    public float spawnRadius = 5f;

    void Start()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle * spawnRadius;
            Instantiate(particlePrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
