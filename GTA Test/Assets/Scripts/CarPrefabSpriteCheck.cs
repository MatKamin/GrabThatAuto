using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteValidator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if the sprite is missing
        if (spriteRenderer.sprite == null)
        {
            Debug.LogError($"The SpriteRenderer on {gameObject.name} does not have a sprite assigned! Please assign a sprite in the Inspector.");
        }
    }

    void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Warn in the Editor when no sprite is assigned
        if (spriteRenderer.sprite == null)
        {
            Debug.LogWarning($"The SpriteRenderer on {gameObject.name} is missing a sprite. Assign one to make the object functional.");
        }
    }
}
