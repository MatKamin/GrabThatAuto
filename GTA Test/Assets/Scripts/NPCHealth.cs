using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [Header("NPC Health Settings")]
    public int maxHP = 100; //%%% Set the maximum HP per NPC here
    private int currentHP;

    [Header("Damage Settings")]
    public int continuousDamage = 10; //%%% Damage taken per second during continuous collision
    private bool isTakingContinuousDamage = false; // Tracks whether the NPC is in a damaging state

    [Header("Bloodlake Settings")]
    public GameObject bloodlakePrefab; //%%% Assign the bloodlake sprite prefab here
    public int bloodlakeFadeTimeMs = 2000; //%%% Time in milliseconds for the bloodlake to fade

    void Start()
    {
        // Initialize current HP
        currentHP = maxHP;
    }

    void Update()
    {
        // Check continuously if health is below 0
        if (currentHP <= 0)
        {
            Die();
        }

        // If NPC is taking continuous damage, reduce health over time
        if (isTakingContinuousDamage)
        {
            TakeContinuousDamage(Time.deltaTime * continuousDamage); // Scale damage by time
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce HP and log the damage
        currentHP -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHP}");

        // Check for death
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void TakeContinuousDamage(float damage)
    {
        // Reduce health over time for continuous damage
        currentHP -= Mathf.RoundToInt(damage); // Convert floating-point damage to integer
        Debug.Log($"{gameObject.name} is taking continuous damage. Current HP: {currentHP}");
    }

    private void Die()
    {
        // Handle NPC death logic
        Debug.Log($"{gameObject.name} has died!");

        // Spawn the bloodlake at the NPC's position
        if (bloodlakePrefab != null)
        {
            GameObject bloodlake = Instantiate(bloodlakePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f))); // Random rotation
            BloodlakeHandler bloodlakeHandler = bloodlake.AddComponent<BloodlakeHandler>();
            bloodlakeHandler.fadeTimeSeconds = bloodlakeFadeTimeMs / 1000f; // Convert ms to seconds
        }
        else
        {
            Debug.LogWarning("Bloodlake prefab not assigned in the Inspector!");
        }

        // Destroy the NPC GameObject
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            float impactForce = collision.relativeVelocity.magnitude; // Speed of the collision
            int damage = Mathf.RoundToInt(impactForce * 10); //%%% Scale damage by speed

            Debug.Log($"{gameObject.name} was hit by a car with speed {impactForce}! Damage: {damage}");

            TakeDamage(damage); // Apply speed-based damage
            isTakingContinuousDamage = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Stop continuous damage when the collision ends
        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log($"{gameObject.name} is no longer colliding with the car.");
            isTakingContinuousDamage = false;
        }
    }
}
