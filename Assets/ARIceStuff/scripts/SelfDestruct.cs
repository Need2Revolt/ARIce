using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destroyTime;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(self, destroyTime);
    }
}
