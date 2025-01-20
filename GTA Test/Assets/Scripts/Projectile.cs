using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; //%%% Adjustable speed of the projectile
    public float range = 10f; //%%% Adjustable maximum range the projectile can travel
    public int damage = 10; //%%% Adjustable damage caused by the projectile
    public LayerMask targetLayer; //%%% Define layers that the projectile interacts with

    private Vector3 startPosition; // Starting position to calculate range

    void Start()
    {
        // Store the starting position of the projectile
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check if the projectile has exceeded its range
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object hit is on the target layer
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            // Apply damage if the object has a health component
            NPCHealth npcHealth = other.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(damage);
                Debug.Log($"Hit {other.name} for {damage} damage!");
            }

            // Destroy the projectile upon hitting a target
            Destroy(gameObject);
        }
    }
}
