using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawnOnHit : MonoBehaviour
{
    public float life = 5f;

    void Update(){

    }
    void OnCollisionEnter(Collision c)
    {
        Debug.Log("hit");
        Destroy(gameObject,life);
    }
}
