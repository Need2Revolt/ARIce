// 15/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SetARMeshLayer : MonoBehaviour
{
    public string targetLayerName = "AR Meshes";
    private ARMeshManager arMeshManager;

    void Awake()
    {
        arMeshManager = GetComponent<ARMeshManager>();
        if (arMeshManager == null)
        {
            Debug.LogError("ARMeshManager not found on this GameObject.");
            enabled = false;
        }
    }

    void OnEnable()
    {
        arMeshManager.meshesChanged += OnMeshesChanged;
    }

    void OnDisable()
    {
        arMeshManager.meshesChanged -= OnMeshesChanged;
    }

    private void OnMeshesChanged(ARMeshesChangedEventArgs args)
    {
        int targetLayer = LayerMask.NameToLayer(targetLayerName);
        if (targetLayer == -1)
        {
            Debug.LogError($"Layer '{targetLayerName}' does not exist. Please create it in the Tags and Layers settings.");
            return;
        }

        foreach (var mesh in args.added)
        {
            mesh.gameObject.layer = targetLayer;
            Debug.Log($"Set layer of {mesh.name} to {targetLayerName}");
        }
    }
}
