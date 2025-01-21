using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 10f;
    public float deceleration = 5f;
    public float maxSpeed = 20f;
    public float turnSpeed = 100f;
    public float spriteRotationOffset = 90f;

    [Header("Trigger Settings")]
    public Vector2 defaultTriggerSize = new Vector2(1.5f, 1.0f);
    public Vector2 expandedTriggerSize = new Vector2(2.0f, 1.0f);

    [Header("Health Settings")]
    public int maxCarHealth = 100; // Maximum car health
    private int currentCarHealth;

    [Header("Player Reference")]
    public Player player; // Reference to the Player script

    public GameObject destroyedCarPrefab; // Prefab to spawn when the car is destroyed
    public int playerDamageOnCarDestruction = 20; // Player damage on car destruction

    private Rigidbody2D rb;
    private BoxCollider2D triggerCollider;
    private float currentSpeed = 0f;
    private float steeringInput = 0f;
    private bool isActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        triggerCollider = GetComponent<BoxCollider2D>();
        currentCarHealth = maxCarHealth; // Initialize car health

        rb.bodyType = RigidbodyType2D.Kinematic;
        AdjustTriggerSize(false); // Default state
    }

    void Update()
    {
        if (isActive)
        {
            float forwardInput = Input.GetAxis("Vertical");
            steeringInput = Input.GetAxis("Horizontal");

            if (forwardInput > 0)
            {
                currentSpeed += forwardInput * acceleration * Time.deltaTime;
            }
            else if (forwardInput < 0)
            {
                currentSpeed += forwardInput * acceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else
        {
            currentSpeed = 0f;
            steeringInput = 0f;
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            Vector2 forwardDirection = Quaternion.Euler(0, 0, spriteRotationOffset) * transform.right;
            Vector2 newPosition = rb.position + forwardDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            if (currentSpeed != 0)
            {
                float direction = Mathf.Sign(currentSpeed);
                float newRotation = rb.rotation - steeringInput * turnSpeed * Time.fixedDeltaTime * direction;
                rb.MoveRotation(newRotation);
            }
        }
    }

    private void AdjustTriggerSize(bool isPlayerInCar)
    {
        if (triggerCollider != null)
        {
            triggerCollider.size = isPlayerInCar ? defaultTriggerSize : expandedTriggerSize;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Street"))
        {
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            Debug.Log("Car hit a wall!");

            // Reduce car health
            int carDamage = Mathf.RoundToInt(Mathf.Abs(currentSpeed) * 20); // Higher damage for the car
            currentCarHealth -= carDamage;
            Debug.Log($"Car took {carDamage} damage. Current car health: {currentCarHealth}");

            // Reduce player health slightly
            if (player != null)
            {
                int playerDamage = Mathf.RoundToInt(Mathf.Abs(currentSpeed) * 5);
                player.TakeDamage(playerDamage);
                Debug.Log($"Player took {playerDamage} damage from wall collision.");
            }

            // Stop the car
            currentSpeed = 0f;

            // Handle car destruction
            if (currentCarHealth <= 0)
            {
                DestroyCar();
            }
        }
        else if (collision.CompareTag("NPC"))
        {
            NPCHealth npcHealth = collision.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                Debug.Log($"Car hit NPC: {collision.gameObject.name}");

                int damage = Mathf.RoundToInt(Mathf.Abs(currentSpeed) * 50);
                npcHealth.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log($"Car triggered collision with: {collision.gameObject.name}");
            currentSpeed = 0f;
        }
    }

    private void DestroyCar()
    {
        Debug.Log("Car is destroyed!");

        // Spawn destroyed car prefab
        if (destroyedCarPrefab != null)
        {
            Instantiate(destroyedCarPrefab, transform.position, transform.rotation);
        }

        // Handle player ejection via CarInteraction
        CarInteraction carInteraction = player.GetComponent<CarInteraction>();
        if (carInteraction != null)
        {
            carInteraction.ExitCar(); // Eject the player using the existing logic
        }
        else
        {
            Debug.LogWarning("CarInteraction script not found on the player!");
        }

        // Apply damage to the player
        if (player != null)
        {
            player.TakeDamage(playerDamageOnCarDestruction);
        }

        // Destroy the car object
        Destroy(gameObject);
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("NPC") && !collision.CompareTag("Street"))
        {
            currentSpeed = 0f; // Stop the car
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Debug.Log($"Car exited trigger with: {collision.gameObject.name}");
        }
    }

    public void ActivateCar()
    {
        isActive = true;
        currentSpeed = 0f;
        AdjustTriggerSize(true);
    }

    public void DeactivateCar()
    {
        isActive = false;
        currentSpeed = 0f;
        AdjustTriggerSize(false);
    }
}
