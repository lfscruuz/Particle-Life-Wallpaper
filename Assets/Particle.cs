using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector2 velocity;
    public Vector2 postition;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(Random.value, Random.value, Random.value);
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //velocity = new Vector2(0, 0);
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
