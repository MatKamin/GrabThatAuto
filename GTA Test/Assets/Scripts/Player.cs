using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health
    private int currentHealth;

    public PlayerHealthBar healthBar; // Reference to the health bar script

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        healthBar.SetMaxHealth(maxHealth); // Initialize health bar
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below 0
        healthBar.SetHealth(currentHealth); // Update health bar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Handle player death (e.g., game over logic)
    }
}
