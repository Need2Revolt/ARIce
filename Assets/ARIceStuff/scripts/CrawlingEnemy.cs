// 15/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CrawlingEnemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Player transform is not assigned. Please assign it in the inspector.");
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not attached to the enemy.");
        }
    }

    void Update()
    {
        if (player != null && navMeshAgent != null)
        {
            // Set the player's position as the destination
            navMeshAgent.SetDestination(player.position);
        }
    }
}
