using UnityEngine;

public class BloodlakeHandler : MonoBehaviour
{
    public float fadeTimeSeconds = 2f; // Time in seconds for the bloodlake to fade

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("BloodlakeHandler requires a SpriteRenderer on the bloodlake prefab!");
            Destroy(gameObject);
            return;
        }

        // Start the fade-out coroutine
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeTimeSeconds)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTimeSeconds); // Linearly interpolate alpha
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Destroy the bloodlake GameObject after fading
        Destroy(gameObject);
    }
}
