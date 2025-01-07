using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer
    private bool nearCar = false; // Whether the player is near a car
    private bool inCar = false; // Whether the player is currently in a car
    private GameObject currentCar; // Reference to the car the player is near
    public PlayerMovement playerMovementScript; // Reference to the PlayerMovement script
    public float exitDistance = 1.5f; // Distance to the car's exit point
    public float exitAngle = 90f; // Angle (in degrees) to determine the exit position relative to the car

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inCar)
            {
                ExitCar();
            }
            else if (nearCar)
            {
                EnterCar();
            }
        }

        // Update the player's position to stick to the car when in the car
        if (inCar && currentCar != null)
        {
            transform.position = currentCar.transform.position; // Place the player on top of the car
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Car"))
        {
            nearCar = true;
            currentCar = collision.gameObject;
            Debug.Log("Player is near a car: " + currentCar.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Car"))
        {
            nearCar = false;
            currentCar = null;
            Debug.Log("Player is no longer near a car.");
        }
    }

    private void EnterCar()
    {
        // Hide the player's sprite
        playerSpriteRenderer.enabled = false;
        // Disable the PlayerMovement script
        playerMovementScript.enabled = false;

        // Enable the CarMovement script
        CarMovement carMovement = currentCar.GetComponent<CarMovement>();
        if (carMovement != null)
        {
            carMovement.ActivateCar();
        }

        // Place the player on top of the car
        transform.position = currentCar.transform.position;

        inCar = true;
        Debug.Log("Player entered the car: " + currentCar.name);
    }

    private void ExitCar()
    {
        // Make the player's sprite visible again
        playerSpriteRenderer.enabled = true;

        // Enable the PlayerMovement script
        playerMovementScript.enabled = true;

        // Disable the CarMovement script
        CarMovement carMovement = currentCar.GetComponent<CarMovement>();
        if (carMovement != null)
        {
            carMovement.DeactivateCar();
        }

        // Place the player at a specified angle relative to the car
        if (currentCar != null)
        {
            Vector3 carPosition = currentCar.transform.position;

            // Calculate the exit position based on the car's rotation and exitAngle
            Vector3 exitDirection = Quaternion.Euler(0, 0, currentCar.transform.eulerAngles.z + exitAngle) * Vector3.right;
            Vector3 exitPosition = carPosition + exitDirection.normalized * exitDistance;

            transform.position = exitPosition; // Move the player to the exit position
        }

        inCar = false;
        Debug.Log("Player exited the car: " + currentCar.name);
    }
}
