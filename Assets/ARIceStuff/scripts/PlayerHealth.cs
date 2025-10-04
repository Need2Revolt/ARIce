using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public Text healthText;

    public void DecreaseHealth(float damage) {
        health -= damage;
        //Debug.Log("N2R: health = " + health);
        healthText.text = "Health: " + health;
    }
}
