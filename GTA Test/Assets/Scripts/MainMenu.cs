using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class MainMenu : MonoBehaviour
{
    // Start the game by loading the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace with your game's scene name
    }

    // Load the settings scene
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene"); // Replace with your settings scene name
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // This will only work in a built application
    }
}
