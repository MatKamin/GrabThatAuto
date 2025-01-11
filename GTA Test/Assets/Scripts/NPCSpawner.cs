using UnityEngine;
using System.Collections.Generic;
using Pathfinding;


public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawning Settings")]
    public GameObject npcPrefab; // NPC prefab to spawn
    public int npcCount = 5; // Number of NPCs to spawn
    public GameObject mainMap; // Main map object containing BoxCollider2D

    [Header("Patrol Settings")]
    public int destinationsPerNPC = 5; // Number of patrol destinations per NPC
    public GameObject destinationPrefab; // Prefab for the patrol destinations

    private BoxCollider2D[] mapColliders; // Store all BoxCollider2D components
    private List<Vector3> destinationPoints; // Stores generated patrol destinations

    void Start()
    {
        if (npcPrefab == null || mainMap == null || destinationPrefab == null)
        {
            Debug.LogError("NPC Prefab, Main Map, or Destination Prefab is not assigned!");
            return;
        }

        // Get all BoxCollider2D components from the mainMap
        mapColliders = mainMap.GetComponents<BoxCollider2D>();
        if (mapColliders == null || mapColliders.Length == 0)
        {
            Debug.LogError("Main Map does not contain any BoxCollider2D components!");
            return;
        }

        // Generate destination points
        GenerateDestinationPoints();

        // Spawn NPCs at valid spawn points
        SpawnNPCs();
    }

    void GenerateDestinationPoints()
    {
        destinationPoints = new List<Vector3>();

        foreach (var collider in mapColliders)
        {
            Bounds bounds = collider.bounds;

            for (int i = 0; i < destinationsPerNPC; i++)
            {
                Vector3 randomPoint = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    0 // Ensure z-axis is zero for 2D
                );

                // Snap the point to be within the collider bounds
                randomPoint.x = Mathf.Clamp(randomPoint.x, bounds.min.x, bounds.max.x);
                randomPoint.y = Mathf.Clamp(randomPoint.y, bounds.min.y, bounds.max.y);

                // Add the destination point and instantiate its prefab
                destinationPoints.Add(randomPoint);
                Instantiate(destinationPrefab, randomPoint, Quaternion.identity);
            }
        }
    }

    void SpawnNPCs()
    {
        if (destinationPoints == null || destinationPoints.Count == 0)
        {
            Debug.LogError("No destination points available for spawning NPCs.");
            return;
        }

        for (int i = 0; i < npcCount; i++)
        {
            Vector3 spawnPoint;
            int maxAttempts = 100; // Avoid infinite loops
            int attempts = 0;

            do
            {
                // Generate a random point within the world bounds
                spawnPoint = GenerateRandomPointOutsideColliders();
                attempts++;
            }
            while (IsPointInsideColliders(spawnPoint) && attempts < maxAttempts);

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Failed to find a valid spawn point for an NPC after multiple attempts.");
                continue;
            }

            // Spawn the NPC at the chosen spawn point
            GameObject npc = Instantiate(npcPrefab, spawnPoint, Quaternion.identity);

            // Assign patrol destinations to the NPC
            AssignPatrolTargets(npc);
        }
    }

    void AssignPatrolTargets(GameObject npc)
    {
        Patrol patrol = npc.GetComponent<Patrol>();
        if (patrol == null)
        {
            Debug.LogError("NPC does not have a Patrol component attached!");
            return;
        }

        // Assign a subset of destination points to this NPC's patrol targets
        List<Transform> npcTargets = new List<Transform>();
        for (int i = 0; i < destinationsPerNPC; i++)
        {
            if (destinationPoints.Count == 0) break;

            // Randomly select a destination point
            int randomIndex = Random.Range(0, destinationPoints.Count);
            Vector3 point = destinationPoints[randomIndex];

            // Create a GameObject for the target and assign its Transform
            GameObject targetObject = new GameObject("PatrolTarget");
            targetObject.transform.position = point;
            npcTargets.Add(targetObject.transform);

            // Remove the used point to avoid duplication
            destinationPoints.RemoveAt(randomIndex);
        }

        patrol.targets = npcTargets.ToArray();
    }

    Vector3 GenerateRandomPointOutsideColliders()
    {
        // Generate a random point in the general area
        Bounds totalBounds = mapColliders[0].bounds;
        foreach (var collider in mapColliders)
        {
            totalBounds.Encapsulate(collider.bounds);
        }

        return new Vector3(
            Random.Range(totalBounds.min.x, totalBounds.max.x),
            Random.Range(totalBounds.min.y, totalBounds.max.y),
            0
        );
    }

    bool IsPointInsideColliders(Vector3 point)
    {
        foreach (var collider in mapColliders)
        {
            if (collider.bounds.Contains(point))
            {
                return true;
            }
        }
        return false;
    }
}