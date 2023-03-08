using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    void Update(){

    }
    void OnCollisionEnter(Collision c)
    {
        Debug.Log("hit");
        Destroy(gameObject);
    }
}
