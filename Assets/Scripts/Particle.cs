using UnityEngine;

public class Particle : MonoBehaviour
{
    public int type;
    public Color color;
    public Vector2 velocity;
    public Vector2 position;
    private SpriteRenderer sr;
    public ParticleManager manager;

    public float K = 0.05f;
    public float friction = 0.95f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        position = transform.position;
    }

    void Update()
    {
        Vector2 totalForce = Vector2.zero;

        float[,] minDistances = manager.minDistances;
        float[,] maxDistances = manager.maxDistances;
        float[,] forces = manager.forces;

        float width = manager.worldWidth;
        float height = manager.worldHeight;
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

                if (distance < minDistances[type, p.type])
                {
                    totalForce += -direction * forces[type, p.type] * K;
                }
                else if (distance < maxDistances[type, p.type])
                {
                    totalForce += direction * forces[type, p.type] * K;
                }

                totalForce += UnityEngine.Random.insideUnitCircle * 0.02f;

                Vector2 tangent = new Vector2(-direction.y, direction.x);
                totalForce += tangent * 0.01f;

            }
        }

        totalForce += UnityEngine.Random.insideUnitCircle * 0.01f;

        Vector2 acceleration = totalForce;
        velocity += acceleration * Time.deltaTime;
        velocity *= friction;
        velocity = Vector2.ClampMagnitude(velocity, 5f);

        position += velocity * Time.deltaTime;

        position.x = minX + (position.x - minX + width) % width;
        position.y = minY + (position.y - minY + height) % height;

        transform.position = new Vector3(position.x, position.y, 0f);
    }

}
