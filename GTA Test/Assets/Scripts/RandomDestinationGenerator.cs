using UnityEngine;

public class RandomDestinationGenerator : MonoBehaviour
{
    [Header("Inputs")]
    public GameObject plane; // The plane defining the area
    public int destinationCount = 10; // Number of destinations to generate
    public LayerMask collisionLayer; // Layer to check for collisions
    public GameObject destinationPrefab; // Prefab for the destinations

    [Header("Debug Options")]
    public bool drawDebugGizmos = true; // Draw debug gizmos for destinations

    private Bounds planeBounds;

    void Start()
    {
        if (plane == null || destinationPrefab == null)
        {
            Debug.LogError("Plane or Destination Prefab is not assigned!");
            return;
        }

        // Get the bounds of the plane
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        if (planeRenderer == null)
        {
            Debug.LogError("Plane does not have a Renderer component!");
            return;
        }
        planeBounds = planeRenderer.bounds;

        GenerateDestinations();
    }

    void GenerateDestinations()
    {
        int generated = 0;

        while (generated < destinationCount)
        {
            // Generate a random point within the plane bounds
            Vector3 randomPoint = new Vector3(
                Random.Range(planeBounds.min.x, planeBounds.max.x),
                Random.Range(planeBounds.min.y, planeBounds.max.y), // Allow y-axis variation
                Random.Range(planeBounds.min.z, planeBounds.max.z)
            );

            // Check if the random point collides with anything on the collision layer
            if (IsPointValid(randomPoint))
            {
                // Instantiate the destination prefab at the valid position
                Instantiate(destinationPrefab, randomPoint, Quaternion.identity);
                generated++;
            }
        }
    }

    bool IsPointValid(Vector3 point)
    {
        // Check for collisions using a sphere cast (adjust radius as needed)
        float checkRadius = 0.5f; // Radius for collision checking
        return !Physics.CheckSphere(point, checkRadius, collisionLayer);
    }

    void OnDrawGizmos()
    {
        if (drawDebugGizmos && plane != null)
        {
            // Draw the plane bounds in the editor
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(planeBounds.center, planeBounds.size);

            // Draw debug spheres for destinations
            foreach (Transform child in transform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(child.position, 0.2f);
            }
        }
    }
}
