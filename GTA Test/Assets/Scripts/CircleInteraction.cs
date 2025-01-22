using UnityEngine;
using UnityEngine.UI; // For UI Text

public class CircleInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [TextArea] // Enables multiline input in the Inspector
    public string messageOnEnter = "You are standing on the circle!\nPress E to accept.";
    [TextArea]
    public string messageOnExit = "You left the circle.";

    public Text uiText; // Reference to the UI Text element

    private bool playerOnCircle = false;

    void Start()
    {
        if (uiText == null)
        {
            Debug.LogError("UI Text is not assigned! Please assign it in the Inspector.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the collider belongs to the player
        {
            playerOnCircle = true;
            UpdateText(messageOnEnter);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnCircle = false;
            UpdateText(messageOnExit);
        }
    }

    private void UpdateText(string newText)
    {
        if (uiText != null)
        {
            uiText.text = newText;
        }
    }
}
