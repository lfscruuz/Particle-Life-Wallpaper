using UnityEngine;

public class Particle : MonoBehaviour
{
    public int type;
    public Color color;
    public Vector2 velocity;
    public Vector2 position;
    private SpriteRenderer sr;
    public ParticleManager manager;

    public float scaler = 0.005f;
    public float friction = 0.95f;
    public float ratio = 3f;

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

        float width = manager.width;
        float height = manager.height;
        float minX = manager.bottomLeft.x;
        float minY = manager.bottomLeft.y;

        for (int i = 0; i < manager.particleCount; i++)
        {
            Particle p = manager.particles[i];
            if (p != this)
            {
                Vector2 direction = p.position - position;

                if (direction.x > 0.5f * width) direction.x -= width;
                if (direction.x < -0.5f * width) direction.x += width;
                if (direction.y > 0.5f * height) direction.y -= height;
                if (direction.y < -0.5f * height) direction.y += height;

                float distance = direction.magnitude;
                direction.Normalize();

                if (distance < manager.minDistances[type, p.type])
                {
                    var force = direction * forces[type, p.type] * ratio * scaler;
                    totalForce += force;
                }
                else if (distance < manager.maxDistances[type, p.type])
                {
                    var force = direction * forces[type, p.type] * scaler;
                    totalForce += force;
                }
            }
        }

        Vector2 acceleration = totalForce;
        velocity += acceleration * Time.deltaTime;
        velocity *= friction;
        position += velocity * Time.deltaTime;

        position.x = minX + (position.x - minX + width) % width;
        position.y = minY + (position.y - minY + height) % height;

        transform.position = new Vector3(position.x, position.y, 0f);
    }
}
