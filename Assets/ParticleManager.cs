using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleManager : MonoBehaviour
{
    public GameObject particlePrefab;
    public int numTypes = 8;
    public int particleCount = 20000;
    public float spawnRadius = 5f;

    void Start()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle;
            GameObject gameObject = Instantiate(particlePrefab, pos, Quaternion.identity);
            Particle particle = gameObject.GetComponent<Particle>();

            int type = SetParticleType(particle);
            SetParticleColor(particle, type);
        }
    }

    void Update()
    {
        
    }

    int SetParticleType(Particle particle)
    {
        int type = Random.Range(0, numTypes);
        particle.type = type;

        return type;
    }

    void SetParticleColor(Particle particle, int type)
    {
        float hue = (float)type / numTypes;
        particle.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hue, 1f, 1f);
    }
}
