using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float lifetime;
    private float spawnTime;

    private Vector2 startPosition; // Tracks the starting position of the projectile
    private float maxDistance;    // Maximum distance the projectile can travel

    public void Initialize(float projectileSpeed, float projectileDamage, float projectileLifetime, float projectileMaxDistance)
    {
        speed = projectileSpeed;
        damage = projectileDamage;
        lifetime = projectileLifetime;
        maxDistance = projectileMaxDistance;
        spawnTime = Time.time;
        startPosition = transform.position; // Record the starting position
    }

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Destroy the projectile after its lifetime
        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }

        // Check if the projectile has traveled beyond the maximum distance
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hit an enemy with NPCHealth
        NPCHealth npcHealth = other.GetComponent<NPCHealth>();
        if (npcHealth != null)
        {
            // Apply the projectile's damage to the NPC
            npcHealth.TakeDamage(Mathf.RoundToInt(damage)); // Cast damage to int
            Debug.Log($"{other.name} took {damage} damage from projectile.");

            // Destroy the projectile after hitting an NPC
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Projectile hit {other.name}, but it doesn't have NPCHealth.");
        }
    }
}
