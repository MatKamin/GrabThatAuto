using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider component
    public Gradient healthGradient; // Gradient for changing the health bar color
    public Image fillImage; // Reference to the Fill image of the slider

    public void SetMaxHealth(int maxHealth)
    {
        // Set the maximum value of the slider
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        // Set the color of the fill image to full health color
        if (fillImage != null && healthGradient != null)
        {
            fillImage.color = healthGradient.Evaluate(1f);
        }
    }

    public void SetHealth(int currentHealth)
    {
        // Update the slider value
        healthSlider.value = currentHealth;

        // Update the color of the fill image
        if (fillImage != null && healthGradient != null)
        {
            fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
        }
    }
}
