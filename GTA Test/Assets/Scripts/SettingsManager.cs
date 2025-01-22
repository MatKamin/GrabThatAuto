using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider; // The slider for adjusting volume

    private const string VolumeKey = "Volume"; // Key for saving volume in PlayerPrefs
    private const float DefaultVolume = 1.0f;  // Default volume level

    void Start()
    {
        // Load the saved volume from PlayerPrefs or default to DefaultVolume
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);

        // Set the slider and the audio volume to the saved value
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // Apply the saved volume
        AudioListener.volume = savedVolume;
    }

    public void OnVolumeChanged(float newVolume)
    {
        // Update the audio volume in real-time
        AudioListener.volume = newVolume;
    }

    public void SaveSettings()
    {
        // Save the current slider value to PlayerPrefs
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat(VolumeKey, volumeSlider.value);
            PlayerPrefs.Save(); // Ensure the value is written to disk
        }

        Debug.Log("Settings Saved: Volume = " + PlayerPrefs.GetFloat(VolumeKey));
    }

    public void OpenCreditsScene()
    {
        // Load the CreditsScene
        SceneManager.LoadScene("CreditsScene");
        Debug.Log("Opened Credits Scene");
    }


    public void RestoreDefaults()
    {
        // Reset the slider to the default value
        if (volumeSlider != null)
        {
            volumeSlider.value = DefaultVolume;
        }

        // Apply the default volume immediately
        AudioListener.volume = DefaultVolume;

        Debug.Log("Settings restored to default values.");
    }

    public void GoBackToMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Returned to Main Menu");
    }
}
