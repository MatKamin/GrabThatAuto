using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Sprite[] weaponSprites; // List of weapon sprites (knife, pistol, rifle)
    public RuntimeAnimatorController[] weaponAnimators; // List of Animator controllers for each weapon
    public int currentWeaponIndex = 0; // Tracks the currently selected weapon

    [Header("UI References")]
    public Image selectedWeaponUI; // Reference to the SelectedWeapon UI Image

    [Header("Animator Reference")]
    public Animator playerAnimator; // Reference to the Player's Animator component

    [Header("PlayerAttack Reference")]
    public PlayerAttack playerAttack; // Reference to the PlayerAttack script

    void Start()
    {
        // Validate input and set the initial weapon
        if (weaponSprites.Length == 0 || weaponAnimators.Length == 0)
        {
            Debug.LogWarning("Weapon sprites or animators are not properly configured!");
            return;
        }

        if (weaponSprites.Length != weaponAnimators.Length)
        {
            Debug.LogError("Weapon sprites and animators arrays must have the same length!");
            return;
        }

        if (selectedWeaponUI == null)
        {
            Debug.LogError("SelectedWeapon UI Image is not assigned!");
            return;
        }

        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is not assigned!");
            return;
        }

        if (playerAttack == null)
        {
            Debug.LogError("PlayerAttack script is not assigned!");
            return;
        }

        // Initialize with the first weapon
        UpdateWeaponDisplay();
        playerAttack.UpdateAmmoOnWeaponSwitch(currentWeaponIndex); // Notify PlayerAttack
    }

    void Update()
    {
        // Check for TAB key input to switch weapons
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchToNextWeapon();
        }
    }

    private void SwitchToNextWeapon()
    {
        // Increment the weapon index and wrap around if necessary
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponSprites.Length;

        // Update the weapon display and animator
        UpdateWeaponDisplay();
        playerAttack.UpdateAmmoOnWeaponSwitch(currentWeaponIndex); // Notify PlayerAttack
    }

    private void UpdateWeaponDisplay()
    {
        // Update the UI Image with the current weapon sprite
        selectedWeaponUI.sprite = weaponSprites[currentWeaponIndex];
        Debug.Log($"Switched to weapon: {weaponSprites[currentWeaponIndex].name}");

        // Update the player's Animator controller
        playerAnimator.runtimeAnimatorController = weaponAnimators[currentWeaponIndex];
        Debug.Log($"Animator switched to: {weaponAnimators[currentWeaponIndex].name}");
    }
}

