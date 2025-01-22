using System.Collections; // For IEnumerator
using UnityEngine;
using UnityEngine.UI; // For Text UI elements

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string meleeAttackTrigger = "MeleeAttack";
    public string pistolAttackTrigger = "PistolAttack";
    public string rifleAttackTrigger = "RifleAttack";
    public float attackCooldown = 1f; 
    private float lastAttackTime = 0f;

    public Transform attackPoint; 
    public float attackRange = 0.5f; 
    public LayerMask enemyLayers; 
    public int meleeDamage = 20;

    // Projectile-related variables
    public GameObject pistolProjectilePrefab; 
    public GameObject rifleProjectilePrefab; 
    public Transform gunPoint; 
    public float projectileSpeed = 10f; 
    public float projectileDamage = 10f; 
    public float projectileMaxDistance = 15f; 
    public float projectileLifetime = 3f; 

    public int rifleBurstCount = 3; 
    public float burstInterval = 0.1f; 

    public WeaponSwitcher weaponSwitcher;

    // Ammunition-related variables
    public int maxPistolAmmo = 15; // Maximum ammo for pistol
    private int currentPistolAmmo; // Current pistol ammo

    public int maxRifleAmmo = 30; // Maximum ammo for rifle
    private int currentRifleAmmo; // Current rifle ammo

    public Text ammoText; // UI text to display current ammo

    // Sound-related variables
    public AudioClip pistolShotSound; // Sound effect for the pistol shot
    public AudioClip meleeAttackSound; // Sound effect for melee attack
    public AudioClip rifleShotSound; // Sound effect for rifle shot
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        currentPistolAmmo = maxPistolAmmo; // Initialize pistol ammo
        currentRifleAmmo = maxRifleAmmo; // Initialize rifle ammo
        UpdateAmmoUI(); // Update the UI text field
        DebugAnimatorParameters();

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            HandleAttack();
            lastAttackTime = Time.time;
        }
    }

    private void HandleAttack()
    {
        int weaponIndex = weaponSwitcher.currentWeaponIndex;

        switch (weaponIndex)
        {
            case 0: // Knife (Melee)
                MeleeAttack();
                break;

            case 1: // Pistol
                if (HasAmmo(WeaponType.Pistol))
                {
                    FireProjectile(pistolProjectilePrefab);
                    TriggerAttackAnimation(pistolAttackTrigger);
                    UseAmmo(WeaponType.Pistol);
                }
                break;

            case 2: // Rifle (Burst Fire)
                if (HasAmmo(WeaponType.Rifle, rifleBurstCount))
                {
                    StartCoroutine(FireBurst());
                    TriggerAttackAnimation(rifleAttackTrigger);
                    UseAmmo(WeaponType.Rifle, rifleBurstCount);
                }
                else
                {
                    Debug.Log("Not enough ammo for burst fire!");
                }
                break;

            default:
                Debug.LogWarning("Unknown weapon index!");
                break;
        }
    }

    private void MeleeAttack()
    {
        SetTriggerDebug(meleeAttackTrigger);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit enemy: {enemy.name}");
            NPCHealth npcHealth = enemy.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(meleeDamage);
                Debug.Log($"{enemy.name} took {meleeDamage} melee damage.");
            }
        }

        // Play the melee attack sound
        if (meleeAttackSound != null)
        {
            audioSource.PlayOneShot(meleeAttackSound);
        }
    }

    private void FireProjectile(GameObject projectilePrefab)
    {
        if (projectilePrefab != null && gunPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunPoint.position, gunPoint.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(projectileSpeed, projectileDamage, projectileLifetime, projectileMaxDistance);
            }

            Debug.Log($"Projectile spawned: {projectile.name}");

            // Play the appropriate sound based on the selected weapon
            if (weaponSwitcher.currentWeaponIndex == 1 && pistolShotSound != null) // Pistol
            {
                audioSource.PlayOneShot(pistolShotSound);
            }
            else if (weaponSwitcher.currentWeaponIndex == 2 && rifleShotSound != null) // Rifle
            {
                audioSource.PlayOneShot(rifleShotSound);
            }
        }
    }


    private IEnumerator FireBurst()
    {
        for (int i = 0; i < rifleBurstCount; i++)
        {
            FireProjectile(rifleProjectilePrefab);
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void UpdateAmmoUI()
    {
        if (weaponSwitcher.currentWeaponIndex == 1) // Pistol
        {
            ammoText.text = $"Pistol Ammo: {currentPistolAmmo}";
        }
        else if (weaponSwitcher.currentWeaponIndex == 2) // Rifle
        {
            ammoText.text = $"Rifle Ammo: {currentRifleAmmo}";
        }
        else
        {
            ammoText.text = "Melee";
        }
    }

    private bool HasAmmo(WeaponType weaponType, int amount = 1)
    {
        if (weaponType == WeaponType.Pistol)
        {
            return currentPistolAmmo >= amount;
        }
        else if (weaponType == WeaponType.Rifle)
        {
            return currentRifleAmmo >= amount;
        }
        return false;
    }

    private void UseAmmo(WeaponType weaponType, int amount = 1)
    {
        if (weaponType == WeaponType.Pistol)
        {
            currentPistolAmmo -= amount;
            currentPistolAmmo = Mathf.Max(currentPistolAmmo, 0);
        }
        else if (weaponType == WeaponType.Rifle)
        {
            currentRifleAmmo -= amount;
            currentRifleAmmo = Mathf.Max(currentRifleAmmo, 0);
        }
        UpdateAmmoUI();
    }

    private void TriggerAttackAnimation(string triggerName)
    {
        SetTriggerDebug(triggerName);
    }

    private void SetTriggerDebug(string triggerName)
    {
        Debug.Log($"Setting Trigger: {triggerName}");
        animator.SetTrigger(triggerName);
    }

    private void DebugAnimatorParameters()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            Debug.Log($"Parameter: {parameter.name}, Type: {parameter.type}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void UpdateAmmoOnWeaponSwitch(int weaponIndex)
    {
        if (weaponIndex == 1) // Pistol
        {
            ammoText.text = $"Pistol Ammo: {currentPistolAmmo}";
        }
        else if (weaponIndex == 2) // Rifle
        {
            ammoText.text = $"Rifle Ammo: {currentRifleAmmo}";
        }
        else
        {
            ammoText.text = "Melee";
        }
    }

    private enum WeaponType
    {
        Pistol,
        Rifle
    }
}
