using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoundaryManager : MonoBehaviour
{
    // This function will be called when another collider enters the trigger or collision area
    void OnCollisionEnter(Collision collision)
    {
        TeleportToNavMesh(collision.gameObject);
    }

    // Teleports the collided object to the nearest point on the NavMesh
    void TeleportToNavMesh(GameObject objectToTeleport)
    {
        Vector3 targetPosition = objectToTeleport.transform.position;

        NavMeshHit hit;
        // Sample the closest point on the NavMesh to the object's position
        if (NavMesh.SamplePosition(targetPosition, out hit, 1000f, NavMesh.AllAreas))
        {
            // Teleport the object to the found position on the NavMesh
            objectToTeleport.transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("No valid point found on NavMesh for " + objectToTeleport.name);
        }
    }
}
