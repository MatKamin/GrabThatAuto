using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect; // The ScrollRect component to scroll
    public float scrollSpeed = 20f; // Speed of scrolling (adjust as needed)
    private bool isScrolling = true; // Flag to control scrolling

    void Start()
    {
        if (scrollRect != null)
        {
            // Disable user interaction with the ScrollRect
            scrollRect.horizontal = false; // Disable horizontal scrolling
            scrollRect.vertical = false;   // Disable vertical scrolling
        }
    }

    void Update()
    {
        if (scrollRect != null && isScrolling)
        {
            // Scroll the content downwards
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime / 100f;

            // Check if the last line is visible
            if (scrollRect.verticalNormalizedPosition <= 0f)
            {
                scrollRect.verticalNormalizedPosition = 0f;
                isScrolling = false; // Stop scrolling
            }
        }
    }

    public void RestartScroll()
    {
        // Optionally restart the scrolling if needed
        scrollRect.verticalNormalizedPosition = 1f;
        isScrolling = true;
    }

    public void GoToSettings()
    {
        // Load the Settings scene
        SceneManager.LoadScene("SettingsScene");
    }
}
