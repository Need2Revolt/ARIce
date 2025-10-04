using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public GameObject self;

    public float damage = 5f;

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.tag == "Player")
        {
            //Decrease a variable amount of health and then self distructs
            //Debug.Log("N2R: Decrease health");
            collision.gameObject.GetComponent<PlayerHealth>().DecreaseHealth(damage);
            //do i want to add like an explosion effect? it will be in your face most of the times,
            //so maybe it's not useful...
            Destroy(self, 0);
        }
    }
}
