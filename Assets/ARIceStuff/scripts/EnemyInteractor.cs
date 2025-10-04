using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyInteractor : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject self;

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

    //when pointer click, enemy has been shot and killed
    public void OnPointerClick(PointerEventData eventData)
    {
        //add enemies that don't die immediately?
        //Debug.Log("N2R: about to remove enemy");
        enemySpawner.removeEnemy(self);
        killsText.text = "Kills: " + enemySpawner.getKills();
        Destroy(self, 0);
        //Debug.Log("N2R: remove enemy complete");
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
