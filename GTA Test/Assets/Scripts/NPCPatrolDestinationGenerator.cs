using UnityEngine;
using System.Collections.Generic;
using Pathfinding; // Add this to access the Patrol class

public class NPCPatrolDestinationGenerator : MonoBehaviour
{
    [Header("Inputs")]
    public GameObject plane; // Plane defining the area
    public int destinationCount = 5; // Number of destinations to generate
    public LayerMask collisionLayer; // Layer to avoid when generating destinations
    public GameObject destinationPrefab; // Prefab for the destinations

    private Patrol patrolScript; // Reference to the Patrol script

    void Start()
    {
        // Get the Patrol script attached to this NPC
        patrolScript = GetComponent<Patrol>();
        if (patrolScript == null)
        {
            Debug.LogError("No Patrol script found on this NPC!");
            return;
        }

        if (plane == null || destinationPrefab == null)
        {
            Debug.LogError("Plane or Destination Prefab is not assigned!");
            return;
        }

        // Generate patrol destinations
        List<Transform> generatedDestinations = GenerateDestinations();

        // Assign the generated destinations to the Patrol script
        patrolScript.targets = generatedDestinations.ToArray();
    }

    List<Transform> GenerateDestinations()
    {
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        if (planeRenderer == null)
        {
            Debug.LogError("Plane does not have a Renderer component!");
            return null;
        }

        Bounds planeBounds = planeRenderer.bounds;
        List<Transform> destinations = new List<Transform>();

        int generated = 0;
        while (generated < destinationCount)
        {
            // Generate a random point within the plane bounds
            Vector3 randomPoint = new Vector3(
                Random.Range(planeBounds.min.x, planeBounds.max.x),
                Random.Range(planeBounds.min.y, planeBounds.max.y),
                Random.Range(planeBounds.min.z, planeBounds.max.z)
            );

            // Check if the point is valid (not colliding)
            if (IsPointValid(randomPoint))
            {
                // Instantiate the destination prefab
                GameObject destination = Instantiate(destinationPrefab, randomPoint, Quaternion.identity);

                // Add the destination transform to the list
                destinations.Add(destination.transform);

                generated++;
            }
        }

        return destinations;
    }

    bool IsPointValid(Vector3 point)
    {
        // Check for collisions using a sphere cast
        float checkRadius = 0.5f; // Radius for collision checking
        return !Physics.CheckSphere(point, checkRadius, collisionLayer);
    }
}
