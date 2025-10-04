// 21/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Unity.XR.XREAL.Samples
{
    public class MeshClassificationFrackingBroken : MonoBehaviour
    {
        const string TAG = "MeshClassificationFracking";
        const int k_NumClassifications = 12;

        public ARMeshManager m_MeshManager;

        [SerializeField]
        private LabelMeshFilterPair[] m_ClassifiedMeshFilterPrefabs;

        private NRMeshingVertexSemanticLabel[] m_AvailableLabels = null;
        private NRMeshingVertexSemanticLabel[] AvailableLabels
        {
            get
            {
                if (m_AvailableLabels == null || m_AvailableLabels.Length == 0)
                {
                    m_AvailableLabels = new NRMeshingVertexSemanticLabel[m_ClassifiedMeshFilterPrefabs.Length];
                    for (int i = 0; i < m_ClassifiedMeshFilterPrefabs.Length; ++i)
                    {
                        m_AvailableLabels[i] = m_ClassifiedMeshFilterPrefabs[i].label;
                    }
                }
                return m_AvailableLabels;
            }
        }

        readonly Dictionary<TrackableId, Dictionary<NRMeshingVertexSemanticLabel, MeshFilter>> m_MeshFrackingMap = new Dictionary<TrackableId, Dictionary<NRMeshingVertexSemanticLabel, MeshFilter>>();

        private Dictionary<TrackableId, MeshChangeState> m_MeshChangeStateMap = new Dictionary<TrackableId, MeshChangeState>();

        readonly List<int> m_BaseTriangles = new List<int>();
        readonly List<Color32> m_BaseColors = new List<Color32>();

        readonly Dictionary<NRMeshingVertexSemanticLabel, List<int>> m_LabelClassifiedTrianglesDict = new Dictionary<NRMeshingVertexSemanticLabel, List<int>>();

        private readonly HashSet<NRMeshingVertexSemanticLabel> allowedLabels = new HashSet<NRMeshingVertexSemanticLabel>
        {
            NRMeshingVertexSemanticLabel.Wall,
            NRMeshingVertexSemanticLabel.Table,
            NRMeshingVertexSemanticLabel.Floor,
            NRMeshingVertexSemanticLabel.Door,
            NRMeshingVertexSemanticLabel.Background
        };

        void OnEnable()
        {
            Debug.Assert(m_MeshManager != null, "mesh manager cannot be null");
            m_MeshManager.meshesChanged += OnMeshesChanged;
        }

        void OnDisable()
        {
            Debug.Assert(m_MeshManager != null, "mesh manager cannot be null");
            m_MeshManager.meshesChanged -= OnMeshesChanged;
        }

        void OnMeshesChanged(ARMeshesChangedEventArgs args)
        {
            foreach (var key in m_MeshChangeStateMap.Keys.ToList())
            {
                m_MeshChangeStateMap[key] = MeshChangeState.Unchanged;
            }

            if (args.added != null)
            {
                args.added.ForEach(BreakupMesh);
            }

            if (args.updated != null)
            {
                args.updated.ForEach(UpdateMesh);
            }

            if (args.removed != null)
            {
                args.removed.ForEach(RemoveMesh);
            }
        }

        void BreakupMesh(MeshFilter meshFilter)
        {
            XRMeshSubsystem meshSubsystem = m_MeshManager.subsystem as XRMeshSubsystem;
            if (meshSubsystem == null)
            {
                return;
            }

            var meshId = ExtractTrackableId(meshFilter.name);
            m_MeshChangeStateMap[meshId] = MeshChangeState.Added;
            var faceClassifications = meshSubsystem.GetFaceClassifications(meshId, Allocator.Persistent);

            if (!faceClassifications.IsCreated)
            {
                return;
            }

            using (faceClassifications)
            {
                if (faceClassifications.Length <= 0)
                {
                    return;
                }

                var parent = meshFilter.transform.parent;

                Dictionary<NRMeshingVertexSemanticLabel, MeshFilter> meshFilters = new Dictionary<NRMeshingVertexSemanticLabel, MeshFilter>();
                for (int i = 0; i < m_ClassifiedMeshFilterPrefabs.Length; ++i)
                {
                    var pair = m_ClassifiedMeshFilterPrefabs[i];
                    if (allowedLabels.Contains(pair.label)) // Only instantiate allowed labels
                    {
                        var filter = (pair.meshFilter == null) ? null : Instantiate(pair.meshFilter, parent);
                        filter.gameObject.name = $"{meshId}_{pair.label}";
                        meshFilters[pair.label] = filter;
                    }
                }
                m_MeshFrackingMap[meshId] = meshFilters;

                var baseMesh = meshFilter.sharedMesh;
                ExtractClassifiedMesh(baseMesh, faceClassifications, meshFilters);
            }
        }

        private void ExtractClassifiedMesh(Mesh baseMesh, NativeArray<NRMeshingVertexSemanticLabel> faceClassifications, Dictionary<NRMeshingVertexSemanticLabel, MeshFilter> meshFilters)
        {
            m_BaseTriangles.Clear();
            baseMesh.GetTriangles(m_BaseTriangles, 0);
            baseMesh.GetColors(m_BaseColors);

            m_LabelClassifiedTrianglesDict.Clear();
            foreach (var label in allowedLabels)
            {
                m_LabelClassifiedTrianglesDict[label] = new List<int>();
            }

            for (int i = 0; i < m_BaseTriangles.Count / 3; i++)
            {
                int idx_0 = m_BaseTriangles[i * 3];
                int idx_1 = m_BaseTriangles[i * 3 + 1];
                int idx_2 = m_BaseTriangles[i * 3 + 2];

                NRMeshingVertexSemanticLabel label = faceClassifications[idx_0];
                if (allowedLabels.Contains(label))
                {
                    var classifiedTriangles = m_LabelClassifiedTrianglesDict[label];
                    classifiedTriangles.Add(idx_0);
                    classifiedTriangles.Add(idx_1);
                    classifiedTriangles.Add(idx_2);
                }
            }

            foreach (var label in allowedLabels)
            {
                if (meshFilters.TryGetValue(label, out var filter))
                {
                    var classifiedMesh = filter.mesh;
                    classifiedMesh.Clear();
                    classifiedMesh.SetVertices(baseMesh.vertices);
                    classifiedMesh.SetNormals(baseMesh.normals);
                    classifiedMesh.SetTriangles(m_LabelClassifiedTrianglesDict[label], 0);
                }
            }
        }

        TrackableId ExtractTrackableId(string meshFilterName)
        {
            string[] nameSplit = meshFilterName.Split(' ');
            return new TrackableId(nameSplit[1]);
        }

        void UpdateMesh(MeshFilter meshFilter)
        {
            // Similar to BreakupMesh but for updates
        }

        void RemoveMesh(MeshFilter meshFilter)
        {
            // Handle mesh removal
        }

        [System.Serializable]
        public class LabelMeshFilterPair
        {
            public NRMeshingVertexSemanticLabel label;
            public MeshFilter meshFilter;
        }
    }
}
