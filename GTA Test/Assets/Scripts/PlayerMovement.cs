using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float rotationSpeed = 10f; // Speed of rotation
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private Vector2 movement; // Stores movement input
    private float targetAngle; // The angle we want the player to rotate to
    private Animator animator; // Reference to the Animator
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    public Sprite idleSprite; // The first sprite to display when idle
    public float spriteRotationOffset = -90f; // Offset to align sprite correctly

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        rb.freezeRotation = true; // Disable rotation caused by physics collisions
    }

    void Update()
    {
        // Get input for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Calculate the target angle only if there is movement
        if (movement != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + spriteRotationOffset;
        }

        // Update Animator parameters
        animator.SetFloat("Speed", movement.sqrMagnitude); // Total movement speed

        // Handle idle animation
        if (movement.sqrMagnitude == 0)
        {
            animator.enabled = false; // Disable Animator when idle
            if (idleSprite != null) // Ensure idleSprite is assigned
            {
                spriteRenderer.sprite = idleSprite; // Set to the idle sprite
            }
        }
        else
        {
            animator.enabled = true; // Enable Animator when moving
        }
    }

    void FixedUpdate()
    {
        // Move the player
        Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Smoothly rotate the sprite to face the movement direction
        if (movement != Vector2.zero) // Rotate only when there is movement
        {
            float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            rb.rotation = smoothedAngle;
        }
    }
}
