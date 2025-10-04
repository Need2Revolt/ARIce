using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class name sucks, it evolved into something different i need to decide if i want
// to rename it or refactor it
public class EnemyOrientation : MonoBehaviour
{
    public GameObject projectile;
    private Transform playerTransform;
    private float speed = 500;
    private int axis = 0;
    private float timer;
    private float delay = 1.5f;
    private float launchVelocity = 0.01f;

    private bool alreadyShot = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay) {
            axis++;
            axis = axis%4;
            timer = 0;
            alreadyShot = false;
        }
        
        switch(axis){
            case 0:
                transform.Rotate(speed * Time.deltaTime, 0, 0);
            break;
            case 1:
                transform.Rotate(0, speed * Time.deltaTime, 0);
            break;
            case 2:
                transform.Rotate(0, 0, speed * Time.deltaTime);
            break;
            case 3:
                transform.LookAt(playerTransform);
                if(!alreadyShot) {
                    alreadyShot = true;
                    Shoot();
                }
            break;

            default:
                transform.LookAt(playerTransform);
            break;
        }
    }

    void Shoot() {
        GameObject round = Instantiate(projectile, transform.position, transform.rotation);
        round.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, launchVelocity));
    }
}
