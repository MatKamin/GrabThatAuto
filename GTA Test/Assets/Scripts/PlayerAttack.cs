using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string meleeAttackTrigger = "MeleeAttack"; // Trigger name in Animator for melee attack
    public string pistolAttackTrigger = "PistolAttack"; // Trigger name for pistol attack
    public string rifleAttackTrigger = "RifleAttack"; // Trigger name for rifle attack
    public float attackCooldown = 1f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Tracks the last attack time

    public Transform attackPoint; // Point where the melee attack hits
    public float attackRange = 0.5f; // Range of the melee attack
    public LayerMask enemyLayers; // Layer mask to identify enemies
    public int meleeDamage = 20; //%%% Adjustable melee damage

    // Projectile-related variables
    public GameObject pistolProjectilePrefab; //%%% Prefab for pistol projectiles
    public GameObject rifleProjectilePrefab; //%%% Prefab for rifle projectiles
    public Transform gunPoint; //%%% The point where projectiles are spawned

    // Reference to the WeaponSwitcher script
    public WeaponSwitcher weaponSwitcher; //%%% Assign the WeaponSwitcher instance here

    void Start()
    {
        // Debug all parameters in the Animator
        DebugAnimatorParameters();
    }

    void Update()
    {
        // Check for input and cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            HandleAttack();
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    private void HandleAttack()
    {
        // Determine the current weapon from the WeaponSwitcher
        int weaponIndex = weaponSwitcher.currentWeaponIndex;

        switch (weaponIndex)
        {
            case 0: // Knife (Melee)
                MeleeAttack();
                break;

            case 1: // Pistol
                FireProjectile(pistolProjectilePrefab);
                TriggerAttackAnimation(pistolAttackTrigger);
                Debug.Log("Pistol attack triggered.");
                break;

            case 2: // Rifle
                FireProjectile(rifleProjectilePrefab);
                TriggerAttackAnimation(rifleAttackTrigger);
                Debug.Log("Rifle attack triggered.");
                break;

            default:
                Debug.LogWarning("Unknown weapon index!");
                break;
        }
    }

    private void MeleeAttack()
    {
        // Trigger the melee attack animation
        SetTriggerDebug(meleeAttackTrigger);

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit enemy: {enemy.name}");

            // Check if the enemy has the NPCHealth component
            NPCHealth npcHealth = enemy.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(meleeDamage); // Apply adjustable melee damage
                Debug.Log($"{enemy.name} took {meleeDamage} melee damage.");
            }
        }
    }

    private void FireProjectile(GameObject projectilePrefab)
    {
        if (projectilePrefab != null && gunPoint != null)
        {
            // Instantiate the projectile at the gun point
            GameObject projectile = Instantiate(projectilePrefab, gunPoint.position, gunPoint.rotation);

            // Debugging the projectile spawn
            Debug.Log($"Projectile spawned: {projectile.name}");
        }
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

    // Draw the attack range in the Unity Editor for melee attacks
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
