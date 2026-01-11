using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.XREAL.Samples; 

public class StartingTutorialManager : MonoBehaviour
{
    [SerializeField]
    protected Text m_TitleUI;
    [SerializeField]
    protected Text m_MessageUI;
    [SerializeField]
    protected Button m_ConfirmBtn;
    protected event Action OnConfirm;

    private int step = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_TitleUI.text = "Interaction";
        m_MessageUI.text = "Use your hands, point at the button with the ray and click by pinching index finger and thumb together. It will take you a few tries.";
        m_ConfirmBtn.onClick.AddListener(OnConfirmClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnConfirmClick()
    {
        //TODO switch image too
        switch (step)
        {
            case 0:
                m_TitleUI.text = "Shooting enemies";
                m_MessageUI.text = "To shoot an enemy, use your LEFT hand, point at the enemy with the ray and click by pinching index finger and thumb together.";
                step++;
                break;
            case 1:
                m_TitleUI.text = "Grabbing enemies";
                m_MessageUI.text = "To grab enemies, reach out with your RIGHT hand and close all your fingers around the enemy.";
                step++;
                break;
            case 2:
                m_TitleUI.text = "World mapping";
                m_MessageUI.text = "Slowly look around to map your environment. This will show detected surfaces in different colours. Feel free to move around. In a room make sure all 4 walls and floor are detected. When you are ready, click Start";
                m_ConfirmBtn.GetComponentInChildren<Text>().text = "Start";
                step++;
                break;
            default:
                gameObject.SetActive(false);
                StartCoroutine(HideMeshMaterialsCoroutine());
                //TODO start game engine
                break;
        }
    }

    private System.Collections.IEnumerator HideMeshMaterialsCoroutine()
    {
        yield return new WaitForEndOfFrame(); // Ensure all meshes are created before hiding materials

        // Access the MeshClassificationFracking component
        var meshClassificationScript = FindObjectOfType<MeshClassificationFracking>();
        if (meshClassificationScript == null)
        {
            Debug.LogWarning("MeshClassificationFracking script not found in the scene.");
            yield break;
        }

        // Iterate through the mesh filters in the MeshFrackingMap dictionary
        foreach (var meshFiltersDict in meshClassificationScript.MeshFrackingMap.Values)
        {
            foreach (var meshFilterPair in meshFiltersDict)
            {
                var meshFilter = meshFilterPair.Value;
                if (meshFilter != null && meshFilter.GetComponent<MeshRenderer>() != null)
                {
                    var renderer = meshFilter.GetComponent<MeshRenderer>();
                    foreach (Material material in renderer.materials)
                    {
                        if (material.HasProperty("_Color"))
                        {
                            Color color = material.color;
                            color.a = 0f; // Set alpha to 0 (fully transparent)
                            material.color = color;
                        }
                    }
                }
            }
        }
    }
}
