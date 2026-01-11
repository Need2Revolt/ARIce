// 15/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using Unity.AI.Navigation;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component
    public ARMeshManager arMeshManager;   // Reference to the ARMeshManager for AR environment meshes

    private float updateInterval = 2f;    // Interval for updating the NavMesh
    private float timer = 0f;

    void Start()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is not assigned. Please assign it in the inspector.");
        }

        if (arMeshManager == null)
        {
            Debug.LogError("ARMeshManager is not assigned. Please assign it in the inspector.");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Update the NavMesh at regular intervals
        if (timer >= updateInterval)
        {
            UpdateNavMesh();
            timer = 0f;
        }
    }

    private void UpdateNavMesh()
    {
        if (navMeshSurface != null)
        {
            // Clear and rebuild the NavMesh
            navMeshSurface.BuildNavMesh();
            Debug.Log("N2R: NavMesh updated.");
        }
    }
}
