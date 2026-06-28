using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject particlePrefab;
    [SerializeField] public int particleCount = 1000;
    [SerializeField] public float spawnRadius = 5f;

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
