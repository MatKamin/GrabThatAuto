using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 10f; // Acceleration speed
    public float deceleration = 5f; // Deceleration speed
    public float maxSpeed = 20f; // Maximum forward/backward speed
    public float turnSpeed = 100f; // Speed of turning
    public float spriteRotationOffset = 90f; // Offset for left-facing sprite (in degrees)

    [Header("Trigger Settings")]
    public Vector2 defaultTriggerSize = new Vector2(1.5f, 1.0f); // Default trigger size
    public Vector2 expandedTriggerSize = new Vector2(2.0f, 1.0f); // Expanded trigger size when the player is not in the car

    private Rigidbody2D rb; // Reference to the Rigidbody2D
    private BoxCollider2D triggerCollider; // Reference to the trigger BoxCollider2D
    private float currentSpeed = 0f; // Current speed
    private float steeringInput = 0f; // Input for steering
    private bool isActive = false; // Is the car currently active (being driven)?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        triggerCollider = GetComponent<BoxCollider2D>();

        // Ensure the Rigidbody is kinematic
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Set the initial trigger size
        AdjustTriggerSize(false); // Default state when the player is outside the car
    }

    void Update()
    {
        if (isActive)
        {
            // Get player inputs
            float forwardInput = Input.GetAxis("Vertical"); // W/S or Up/Down keys
            steeringInput = Input.GetAxis("Horizontal"); // A/D or Left/Right keys

            // Accelerate or decelerate the car based on input
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
                // Apply deceleration when no input is given
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            // Clamp the current speed to max limits
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else
        {
            // No input when the car is not active
            currentSpeed = 0f;
            steeringInput = 0f;
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            // Calculate forward direction with sprite rotation offset
            Vector2 forwardDirection = Quaternion.Euler(0, 0, spriteRotationOffset) * transform.right;

            // Move the car using Rigidbody2D
            Vector2 newPosition = rb.position + forwardDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            // Apply steering directly to the rotation
            if (currentSpeed != 0)
            {
                float direction = Mathf.Sign(currentSpeed); // Determine forward or backward
                float newRotation = rb.rotation - steeringInput * turnSpeed * Time.fixedDeltaTime * direction;
                rb.MoveRotation(newRotation);
            }
        }
    }

    // Adjust the size of the trigger collider
    private void AdjustTriggerSize(bool isPlayerInCar)
    {
        if (triggerCollider != null)
        {
            triggerCollider.size = isPlayerInCar ? defaultTriggerSize : expandedTriggerSize;
            Debug.Log($"Trigger size adjusted to: {triggerCollider.size}");
        }
    }

    // Handle trigger collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with the Player tag
        if (collision.CompareTag("Player")) return;

        Debug.Log($"Car triggered collision with: {collision.gameObject.name}");
        currentSpeed = 0f; // Stop the car

        // Optional: Check for specific tags or layers
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with an obstacle!");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Ignore collisions with the Player tag
        if (collision.CompareTag("Player")) return;

        // Handle ongoing trigger collision (optional)
        Debug.Log($"Car is still triggering with: {collision.gameObject.name}");
        currentSpeed = 0f; // Stop the car
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Ignore collisions with the Player tag
        if (collision.CompareTag("Player")) return;

        // Handle when the car exits the collision area (optional)
        Debug.Log($"Car exited trigger with: {collision.gameObject.name}");
    }

    // Public method to activate the car
    public void ActivateCar()
    {
        isActive = true;
        currentSpeed = 0f; // Ensure the car starts stationary
        AdjustTriggerSize(true); // Shrink the trigger size when the player is in the car
    }

    // Public method to deactivate the car
    public void DeactivateCar()
    {
        isActive = false;
        currentSpeed = 0f; // Stop the car immediately
        AdjustTriggerSize(false); // Expand the trigger size when the player is out of the car
    }
}
