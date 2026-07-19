using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleManager : MonoBehaviour
{
    public GameObject particlePrefab;
    public int numTypes = 8;
    public int particleCount = 20000;
    public Particle[] particles;
    public float[,] minDistances;
    public float[,] maxDistances;
    public float[,] forces;

    public float worldWidth;
    public float worldHeight;
    public Vector3 bottomLeft;
    public Vector3 topRight;

    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        worldWidth = topRight.x - bottomLeft.x;
        worldHeight = topRight.y - bottomLeft.y;

        particles = new Particle[particleCount];
        minDistances = new float[numTypes, numTypes];
        maxDistances = new float[numTypes, numTypes];
        forces = new float[numTypes, numTypes];


        SetParameters();

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 viewportPos = new Vector3(Random.value, Random.value, 0);
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
            worldPos.z = 0;

            
            GameObject gameObject = Instantiate(particlePrefab, worldPos, Quaternion.identity);
            Particle particle = gameObject.GetComponent<Particle>();

            particle.manager = this;

            int type = SetParticleType(particle);
            SetParticleColor(particle, type);

            particles[i] = particle;
        }

        Debug.Log("Forces: \n");
        for (int i = 0; i < numTypes; i++)
        {
            for (int j = 0; j < numTypes; j++)
            {
                Debug.Log($"Force of type {i} onto type {j}: {forces[i, j]}\n");
            }
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

    void SetParameters()
    {
        for (int i = 0;i < numTypes;i++)
        {
            for (int j  = 0;j < numTypes;j++)
            {
                forces[i, j] = Random.Range(0.25f, 1f);

                if (Random.Range(0, 100) < 50)
                {
                    forces[i, j] *= -1;
                }
                minDistances[i, j] = Random.Range(30, 50);
                maxDistances[i, j] = Random.Range(70, 250);
            }
        }
    }
}
