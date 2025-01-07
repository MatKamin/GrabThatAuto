using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // The NPC prefab to spawn
    public Transform[] waypoints; // Array of waypoints (Destinations in your setup)
    public Transform[] startPoints; // Array of starting points (StartingPoint in your setup)
    public int npcCount = 10; // Number of NPCs to spawn

    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < npcCount; i++)
        {
            // Get a random starting point
            Transform startPoint = GetRandomTransform(startPoints);

            // Spawn the NPC at the starting point
            GameObject npc = Instantiate(npcPrefab, startPoint.position, Quaternion.identity);

            // Assign a NavMeshAgent and waypoints
            if (npc.TryGetComponent<NPCNavMeshMovement>(out var npcMovement))
            {
                npcMovement.destinations = waypoints; // Assign the waypoints
            }
        }
    }

    Transform GetRandomTransform(Transform[] transforms)
    {
        if (transforms.Length == 0)
        {
            Debug.LogError("No transforms assigned!");
            return null;
        }

        return transforms[Random.Range(0, transforms.Length)];
    }
}
