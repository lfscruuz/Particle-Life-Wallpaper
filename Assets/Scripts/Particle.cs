using UnityEngine;

public class Particle : MonoBehaviour
{
    public int type;
    public Color color;
    public Vector2 velocity;
    public Vector2 position;
    private SpriteRenderer sr;
    public ParticleManager manager;

    public float K = 0.005f;
    public float friction = 0.95f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        position = transform.position;
    }

    void Update()
    {
        var totalForce = new Vector2(0, 0);
        var accelation = new Vector2(0, 0);

        float[,] minDistances = manager.minDistances;
        float[,] maxDistances = manager.maxDistances;
        float[,] forces = manager.forces;

        for (int i = 0; i < manager.particleCount; i++)
        {
            Particle p = manager.particles[i];
            if (p != this)
            {
                Vector2 direction = p.position - position;
                float distance = direction.magnitude;
                direction.Normalize();

                if (distance > manager.minDistances[type, p.type] &&
                    distance < manager.maxDistances[type, p.type])
                {
                    var force = direction * forces[type, p.type];
                    force *= K;
                    totalForce += force;
                }
            }
        }

        Vector2 acceleration = totalForce;
        velocity += acceleration;
        velocity *= friction;
        position += velocity;
        transform.position = new Vector3(position.x, position.y, 0f);
    }
}
