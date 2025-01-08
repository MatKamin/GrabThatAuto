using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawning Settings")]
    public GameObject npcPrefab; // NPC prefab to spawn
    public int npcCount = 5; // Number of NPCs to spawn
    public GameObject plane; // Plane defining the spawn area
    public LayerMask collisionLayer; // Layer to avoid when spawning NPCs

    [Header("Patrol Settings")]
    public int destinationsPerNPC = 5; // Number of patrol destinations per NPC
    public GameObject destinationPrefab; // Prefab for the patrol destinations

    private Bounds planeBounds;

    void Start()
    {
        if (npcPrefab == null || plane == null || destinationPrefab == null)
        {
            Debug.LogError("NPC Prefab, Plane, or Destination Prefab is not assigned!");
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

        // Spawn the NPCs
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        int spawned = 0;

        while (spawned < npcCount)
        {
            // Generate a random spawn point within the plane bounds
            Vector3 spawnPoint = new Vector3(
                Random.Range(planeBounds.min.x, planeBounds.max.x),
                Random.Range(planeBounds.min.y, planeBounds.max.y),
                Random.Range(planeBounds.min.z, planeBounds.max.z)
            );

            // Check if the spawn point is valid (not colliding with obstacles)
            if (IsPointValid(spawnPoint))
            {
                // Spawn the NPC
                GameObject npc = Instantiate(npcPrefab, spawnPoint, Quaternion.identity);

                // Set up the NPC's patrol destinations
                NPCPatrolDestinationGenerator patrolGenerator = npc.AddComponent<NPCPatrolDestinationGenerator>();
                patrolGenerator.plane = plane;
                patrolGenerator.destinationCount = destinationsPerNPC;
                patrolGenerator.collisionLayer = collisionLayer;
                patrolGenerator.destinationPrefab = destinationPrefab;

                spawned++;
            }
        }
    }

    bool IsPointValid(Vector3 point)
    {
        // Check for collisions using a sphere cast
        float checkRadius = 0.5f; // Adjust the radius as needed
        return !Physics.CheckSphere(point, checkRadius, collisionLayer);
    }
}
