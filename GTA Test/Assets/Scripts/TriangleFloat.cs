using UnityEngine;

public class TriangleFloat : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 1f; // Speed of the up and down movement
    public float floatAmount = 0.5f; // Maximum distance to move up and down

    private Vector3 startPosition; // Starting position of the triangle

    void Start()
    {
        // Record the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using Mathf.Sin for smooth oscillation
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        // Apply the new position to the triangle
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
