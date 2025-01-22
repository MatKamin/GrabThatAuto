using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign the PauseMenu panel in the Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Check for the "P" key
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Show the pause menu
        pauseMenu.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;

        isPaused = true;
    }

    public void ResumeGame()
    {
        // Hide the pause menu
        pauseMenu.SetActive(false);

        // Unfreeze the game
        Time.timeScale = 1f;

        isPaused = false;
    }

    public void OpenSettings()
    {
        // Unfreeze the game and load the Settings scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("SettingsScene");
    }

    public void GoToMainMenu()
    {
        // Unfreeze the game and load the Main Menu scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
