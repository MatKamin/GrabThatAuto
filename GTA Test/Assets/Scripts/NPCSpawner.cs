using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawning Settings")]
    public GameObject npcPrefab; // NPC prefab to spawn
    public int npcCount = 5; // Number of NPCs to spawn

    [Header("Spawn Zones")]
    public List<Transform> spawnZones; // Predefined spawn zones with BoxCollider2D

    [Header("Patrol Settings")]
    public int destinationsPerNPC = 5; // Number of patrol destinations per NPC
    public GameObject destinationPrefab; // Prefab for the patrol destinations

    [Header("Collision Settings")]
    public LayerMask wallLayerMask; // LayerMask for "Wall" objects

    void Start()
    {
        if (npcPrefab == null || destinationPrefab == null)
        {
            Debug.LogError("NPC Prefab or Destination Prefab is not assigned!");
            return;
        }

        if (spawnZones == null || spawnZones.Count == 0)
        {
            Debug.LogError("No spawn zones assigned! Add spawn zones in the Inspector.");
            return;
        }

        // Spawn NPCs in predefined zones
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < npcCount; i++)
        {
            Vector3 spawnPoint = GetRandomValidSpawnPoint();

            if (spawnPoint == Vector3.zero)
            {
                Debug.LogWarning("Failed to find a valid spawn point for an NPC.");
                continue;
            }

            // Spawn the NPC at the chosen spawn point
            GameObject npc = Instantiate(npcPrefab, spawnPoint, Quaternion.identity);

            // Generate unique patrol destinations for this NPC
            GeneratePatrolDestinationsForNPC(npc);
        }
    }

    void GeneratePatrolDestinationsForNPC(GameObject npc)
    {
        Patrol patrol = npc.GetComponent<Patrol>();
        if (patrol == null)
        {
            Debug.LogError("NPC does not have a Patrol component attached!");
            return;
        }

        List<Transform> npcTargets = new List<Transform>();

        for (int i = 0; i < destinationsPerNPC; i++)
        {
            // Generate a random valid point in a random spawn zone
            Vector3 point = GetRandomValidSpawnPoint();

            // Create a GameObject for the target and assign its Transform
            GameObject targetObject = Instantiate(destinationPrefab, point, Quaternion.identity);
            targetObject.name = $"PatrolTarget_{npc.name}_{i}"; // Unique name for debugging
            npcTargets.Add(targetObject.transform);
        }

        // Assign the generated targets to the NPC's patrol
        patrol.targets = npcTargets.ToArray();
    }

    Vector3 GetRandomValidSpawnPoint()
    {
        int maxAttempts = 100; // Limit the number of attempts to avoid infinite loops
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;

            // Get a random point in the spawn zones
            Vector3 point = GetRandomPointInSpawnZones();

            // Check if the point overlaps with any "Wall" objects
            if (!IsPointOverlappingWithWalls(point))
            {
                return point; // Valid point found
            }
        }

        Debug.LogWarning("Failed to find a valid spawn point after multiple attempts.");
        return Vector3.zero; // Return zero if no valid point is found
    }

    Vector3 GetRandomPointInSpawnZones()
    {
        if (spawnZones.Count == 0)
        {
            Debug.LogError("No spawn zones available.");
            return Vector3.zero;
        }

        // Choose a random spawn zone
        Transform randomZone = spawnZones[Random.Range(0, spawnZones.Count)];
        BoxCollider2D collider = randomZone.GetComponent<BoxCollider2D>();

        if (collider == null)
        {
            Debug.LogError($"Spawn zone {randomZone.name} does not have a BoxCollider2D!");
            return Vector3.zero;
        }

        // Generate a random point within the bounds of the zone
        Bounds bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0 // Ensure z-axis is zero for 2D
        );
    }

    bool IsPointOverlappingWithWalls(Vector3 point)
    {
        // Check for overlap with any 2D Collider on the "Wall" layer
        Collider2D hit = Physics2D.OverlapPoint(point, wallLayerMask);
        return hit != null; // Returns true if the point overlaps with a "Wall"
    }
}
