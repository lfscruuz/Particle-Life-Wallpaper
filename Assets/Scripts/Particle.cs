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
    public float friction = 0.99f;

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

                // wrap-around adjustment
                if (direction.x > 0.5f * width) direction.x -= width;
                if (direction.x < -0.5f * width) direction.x += width;
                if (direction.y > 0.5f * height) direction.y -= height;
                if (direction.y < -0.5f * height) direction.y += height;

                float distance = direction.magnitude;
                if (distance == 0f) continue; // avoid division by zero
                direction.Normalize();

                // repel if too close
                if (distance < minDistances[type, p.type])
                {
                    totalForce += -direction * forces[type, p.type] * K;
                }
                else if (distance < maxDistances[type, p.type])
                {
                    totalForce += direction * forces[type, p.type] * K;
                }

                // add angular noise
                totalForce += UnityEngine.Random.insideUnitCircle * 0.02f;

                // add tangential component
                Vector2 tangent = new Vector2(-direction.y, direction.x);
                totalForce += tangent * 0.01f;

            }
        }

        // add small angular noise to break straight lines
        totalForce += UnityEngine.Random.insideUnitCircle * 0.01f;

        // physics integration
        Vector2 acceleration = totalForce;
        velocity += acceleration * Time.deltaTime;
        velocity *= friction; // damping
        velocity = Vector2.ClampMagnitude(velocity, 5f);

        position += velocity * Time.deltaTime;

        // wrap around screen edges
        position.x = minX + (position.x - minX + width) % width;
        position.y = minY + (position.y - minY + height) % height;

        transform.position = new Vector3(position.x, position.y, 0f);
    }

}
