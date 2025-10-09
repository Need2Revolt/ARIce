using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EnemyInteractor : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject self;
    public ParticleSystem deathEffect1;
    public ParticleSystem deathEffect2;

    private EnemySpawner enemySpawner;

    private Text killsText;

    void Awake () {
        enemySpawner = GameObject.FindGameObjectsWithTag("Spawner")[0].GetComponent<EnemySpawner>();
        killsText = GameObject.FindGameObjectsWithTag("KillsCount")[0].GetComponent<Text>();
    }

    void Update()
    {
        //Nothing?
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().isSelected)
        {
            Die();
        }
    }

    //when pointer click, enemy has been shot and killed
    public void OnPointerClick(PointerEventData eventData)
    {
        //add enemies that don't die immediately?
        Debug.Log("N2RG: about to remove enemy");
        Die();
        Debug.Log("N2RG: remove enemy complete");
    }

    public void Die()
    {
        enemySpawner.removeEnemy(self);
        killsText.text = "" + enemySpawner.getKills();
        GetComponent<Renderer>().enabled = false;
        deathEffect1.Play();
        deathEffect2.Play();
        Destroy(self, 2);
    }

    //when pointer hover, not sure what to do
    public void OnPointerEnter(PointerEventData eventData)
    {
        //show enemy health?
    }

    //when pointer exit hover, not sure what to do
    public void OnPointerExit(PointerEventData eventData)
    {
        //hide enemy health?
    }

}
