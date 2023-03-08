using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeleteNetworkManeger : MonoBehaviour
{

    public static DeleteNetworkManeger instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
