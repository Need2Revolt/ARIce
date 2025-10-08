// 08/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARMeshManager))]
public class ARMeshColliderManager : MonoBehaviour
{
    private ARMeshManager arMeshManager;

    void Awake()
    {
        // Get the ARMeshManager component
        arMeshManager = GetComponent<ARMeshManager>();
    }

    void OnEnable()
    {
        // Subscribe to the mesh events
        arMeshManager.meshesChanged += OnMeshesChanged;
    }

    void OnDisable() 
    {
        // Unsubscribe from the mesh events
        arMeshManager.meshesChanged -= OnMeshesChanged;
    }

    private void OnMeshesChanged(ARMeshesChangedEventArgs args)
    {
        // Add MeshCollider to newly added meshes
        foreach (var meshFilter in args.added)
        {
            AddMeshCollider(meshFilter);
        }

        // Update MeshCollider for updated meshes
        foreach (var meshFilter in args.updated)
        {
            UpdateMeshCollider(meshFilter);
        }
    }

    private void AddMeshCollider(MeshFilter meshFilter)
    {
        var meshCollider = meshFilter.gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
        }

        // Assign the mesh to the collider
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }

    private void UpdateMeshCollider(MeshFilter meshFilter)
    {
        var meshCollider = meshFilter.gameObject.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            // Update the mesh in the collider
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
    }
}
