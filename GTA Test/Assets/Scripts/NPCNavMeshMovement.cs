using UnityEngine;
using UnityEngine.AI;

public class NPCNavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agent; // NavMeshAgent attached to the NPC
    public Transform[] destinations; // Waypoints where the NPC can go

    private Transform currentDestination;

    void Start()
    {
        if (destinations.Length > 0)
        {
            SetRandomDestination();
        }
        else
        {
            Debug.LogError("No destinations assigned to NPCNavMeshMovement!");
        }
    }

    void Update()
    {
        if (agent != null && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        if (destinations.Length > 0)
        {
            currentDestination = destinations[Random.Range(0, destinations.Length)];
            agent.SetDestination(currentDestination.position);
        }
    }
}
