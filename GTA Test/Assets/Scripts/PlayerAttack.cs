using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string meleeAttackTrigger = "MeleeAttack"; // Trigger name in Animator for melee attack
    public float attackCooldown = 1f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Tracks the last attack time

    public Transform attackPoint; // Point where the melee attack hits
    public float attackRange = 0.5f; // Range of the melee attack
    public LayerMask enemyLayers; // Layer mask to identify enemies
    public int meleeDamage = 20; //%%% Adjustable melee damage

    // Gun-related placeholders for future functionality
    public bool hasGun = false; // Whether the player has a gun
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform gunPoint; // Point where the gun fires bullets
    public float bulletSpeed = 10f; // Speed of the bullet

    void Start()
    {
        // Debug all parameters in the Animator
        DebugAnimatorParameters();
    }

    void Update()
    {
        // Monitor the current Animator state
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // Check for input and cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            if (hasGun)
            {
                Shoot();
            }
            else
            {
                MeleeAttack();
            }
            lastAttackTime = Time.time; // Update the last attack time
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

    private void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
            // Instantiate a bullet at the gun point
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

            // Add velocity to the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = gunPoint.right * bulletSpeed;
            }

            Debug.Log("Shot fired!");
        }
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
