using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleManager : MonoBehaviour
{
    public GameObject particlePrefab;
    public int numTypes = 8;
    public int particleCount = 1000;
    public Particle[] particles;
    public float[,] minDistances;
    public float[,] maxDistances;
    public float[,] forces;

    public float minDistanceRangeMinimum = 1f;
    public float minDistanceRangeMaximum = 1.5f;
    public float maxDistanceRangeMinimum = 2f;
    public float maxDistanceRangeMaximum = 5f;

    public float forcesRangeMinimum = 1f;
    public float forcesRangeMaximum = 5f;

    public Vector3 bottomLeft;
    public Vector3 topRight;
    public float width;
    public float height;

    void Start()
    {
        particles = new Particle[particleCount];
        minDistances = new float[numTypes, numTypes];
        maxDistances = new float[numTypes, numTypes];
        forces = new float[numTypes, numTypes];

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;

        int rows = 50;
        int cols = 90;

        float stepX = width / cols;
        float stepY = height / rows;

        int index = 0;

        Debug.Log($"width: {width}\n");
        Debug.Log($"height: {height}\n");

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
                forces[i, j] = Random.Range(forcesRangeMinimum, forcesRangeMaximum);

                if (Random.Range(0, 100) < 50)
                {
                    forces[i, j] *= -1;
                }
                minDistances[i, j] = Random.Range(minDistanceRangeMinimum, minDistanceRangeMaximum);
                maxDistances[i, j] = Random.Range(maxDistanceRangeMinimum, maxDistanceRangeMaximum);
            }
        }
    }
}
