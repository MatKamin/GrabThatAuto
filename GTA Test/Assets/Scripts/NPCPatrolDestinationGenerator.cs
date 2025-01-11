using UnityEngine;
using System.Collections.Generic;

public class NPCPatrolDestinationGenerator : MonoBehaviour
{
    public GameObject mainMap; // Main map with BoxCollider2D
    public int destinationCount = 5; // Number of destinations to generate
    public GameObject destinationPrefab; // Prefab for the destinations

    void Start()
    {
        if (mainMap == null || destinationPrefab == null)
        {
            Debug.LogError("Main Map or Destination Prefab is not assigned!");
            return;
        }

        // Generate patrol destinations
        GenerateDestinations();
    }

    void GenerateDestinations()
    {
        BoxCollider2D mapCollider = mainMap.GetComponent<BoxCollider2D>();
        if (mapCollider == null)
        {
            Debug.LogError("Main Map does not have a BoxCollider2D component!");
            return;
        }

        Bounds mapBounds = mapCollider.bounds;

        for (int i = 0; i < destinationCount; i++)
        {
            // Generate a random point on top of the map's BoxCollider2D
            Vector3 randomPoint = new Vector3(
                Random.Range(mapBounds.min.x, mapBounds.max.x),
                Random.Range(mapBounds.min.y, mapBounds.max.y),
                0 // Keep the z-axis value zero for 2D
            );

            // Instantiate the destination prefab at the generated point
            Instantiate(destinationPrefab, randomPoint, Quaternion.identity);
        }
    }
}