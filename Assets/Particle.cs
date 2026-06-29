using UnityEngine;

public class Particle : MonoBehaviour
{
    public int type;
    public Color color;
    public Vector2 velocity;
    public Vector2 postition;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //velocity = new Vector2(0, 0);
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
