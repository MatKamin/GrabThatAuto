using UnityEngine;
using UnityEngine.UI; // For handling UI elements

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health
    private int currentHealth;

    [Header("Currency and XP")]
    public int heistBucks = 0; // Player's money
    public int xp = 0; // Player's XP

    [Header("UI References")]
    public PlayerHealthBar healthBar; // Reference to the health bar script
    public Text heistBucksText; // UI text for money display
    public Text xpText; // UI text for XP display

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); // Initialize health bar

        // Initialize UI elements
        UpdateHeistBucksUI();
        UpdateXPUI();
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

    public void AddHeistBucks(int amount)
    {
        heistBucks += amount;
        UpdateHeistBucksUI();
    }

    public void AddXP(int amount)
    {
        xp += amount;
        UpdateXPUI();
    }

    private void UpdateHeistBucksUI()
    {
        if (heistBucksText != null)
        {
            heistBucksText.text = $"${heistBucks}";
        }
        else
        {
            Debug.LogWarning("Heist Bucks Text is not assigned in the Inspector!");
        }
    }

    private void UpdateXPUI()
    {
        if (xpText != null)
        {
            xpText.text = $"XP: {xp}";
        }
        else
        {
            Debug.LogWarning("XP Text is not assigned in the Inspector!");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Handle player death (e.g., game over logic)
    }
}
