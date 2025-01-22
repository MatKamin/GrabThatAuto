using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    [Header("Mission Settings")]
    [TextArea]
    public string objectiveText = "Kill all targets"; // Objective description
    public int requiredKills = 5; // Number of NPCs to kill
    public float missionTimeLimit = 120f; // Mission timer in seconds

    [Header("Rewards")]
    public int rewardXP = 100; // XP reward for completing the mission
    public int rewardMoney = 50; // Money reward for completing the mission

    [Header("UI Elements")]
    public Text missionText; // Text element for mission display
    public Text timerText; // Text element for timer display

    [Header("Player Reference")]
    public Player player; // Reference to the Player script

    [Header("Mission Marker")]
    public GameObject missionMarker; // The mission marker object

    [Header("Mission Activation Zone")]
    public GameObject activationZone; // The object the player must stand on to trigger the mission

    [Header("Audio Settings")]
    public AudioClip missionStartSound; // Sound for mission start
    public AudioClip missionSuccessSound; // Sound for mission success
    public AudioClip missionFailSound; // Sound for mission failure
    private AudioSource audioSource; // Audio source for playing sounds

    private bool missionActive = false;
    private float missionTimer;
    private int currentKills = 0;

    void Start()
    {
        if (missionText == null || timerText == null)
        {
            Debug.LogError("Mission Text or Timer Text is not assigned! Please assign them in the Inspector.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned! Please assign it in the Inspector.");
            return;
        }

        if (missionMarker == null)
        {
            Debug.LogError("Mission Marker is not assigned! Please assign it in the Inspector.");
            return;
        }

        if (activationZone == null)
        {
            Debug.LogError("Activation Zone is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide UI elements initially
        missionText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Start the mission when the player presses E and is in the activation zone
        if (!missionActive && Input.GetKeyDown(KeyCode.E) && IsPlayerInActivationZone())
        {
            StartMission();
        }

        // Update mission timer and check if time is up
        if (missionActive)
        {
            missionTimer -= Time.deltaTime;

            if (missionTimer <= 0)
            {
                EndMission(false);
            }

            // Update only the timer text
            UpdateTimerText();
        }
    }

    public void RegisterKill()
    {
        if (!missionActive) return; // Only count kills when mission is active

        currentKills++;
        missionText.text = $"Objective:\n- {objectiveText} [{currentKills} / {requiredKills}]";

        if (currentKills >= requiredKills)
        {
            EndMission(true);
        }
    }

    private void StartMission()
    {
        missionActive = true;
        missionTimer = missionTimeLimit;
        currentKills = 0;

        Debug.Log("Mission started!");

        // Play mission start sound
        PlaySound(missionStartSound);

        // Hide mission marker
        if (missionMarker != null)
        {
            missionMarker.SetActive(false); // Only hide the marker
        }

        // Show UI elements
        missionText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        missionText.text = $"Objective:\n- {objectiveText} [0 / {requiredKills}]";
    }

    private void EndMission(bool success)
    {
        missionActive = false;

        if (success)
        {
            missionText.text = "Mission Complete!";
            Debug.Log("Mission accomplished!");

            // Reward the player
            player.AddXP(rewardXP);
            player.AddHeistBucks(rewardMoney);

            Debug.Log($"Player received {rewardXP} XP and {rewardMoney} Heist Bucks.");

            // Play mission success sound
            PlaySound(missionSuccessSound);

            // Delete the mission marker
            if (missionMarker != null)
            {
                Destroy(missionMarker);
            }
        }
        else
        {
            missionText.text = "Mission Failed!";
            Debug.Log("Mission failed!");

            // Play mission fail sound
            PlaySound(missionFailSound);

            // Re-enable the mission marker
            if (missionMarker != null)
            {
                missionMarker.SetActive(true);
            }
        }

        // Hide UI elements after a short delay
        StartCoroutine(HideUIAfterDelay());
    }

    private void UpdateTimerText()
    {
        // Format timer as mm:ss
        int minutes = Mathf.FloorToInt(missionTimer / 60);
        int seconds = Mathf.FloorToInt(missionTimer % 60);

        string formattedTime = $"{minutes:D2}:{seconds:D2}";
        timerText.text = $"Timer: {formattedTime}";
    }

    private IEnumerator HideUIAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Optional delay before hiding UI
        missionText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    private bool IsPlayerInActivationZone()
    {
        // Check if the player is inside the activation zone
        Collider2D zoneCollider = activationZone.GetComponent<Collider2D>();

        if (zoneCollider == null)
        {
            Debug.LogError("Activation zone does not have a Collider2D component!");
            return false;
        }

        // Check if the player's collider is overlapping the activation zone's collider
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        return zoneCollider.bounds.Intersects(playerCollider.bounds);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
