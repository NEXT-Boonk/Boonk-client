using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Net.NetworkInformation;
using Unity.Netcode.Transports.UTP;
using System; 
using System.Linq;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;


public class NetworkPlayerSpawner : MonoBehaviour
{
    

    // Deifining Unity transport
    UnityTransport UT;
    Data data;
    string ip;
    string port;

    private void Awake(){

        data = FindObjectOfType<Data>();
        UT = FindObjectOfType<UnityTransport>();


        try { 
			UT.ConnectionData.Port = UInt16.Parse(data.GetPort());
			UT.ConnectionData.Address = data.GetIp();
		} catch(Exception error) {
            Debug.LogError("Could not connect to server: " + error);
            return;
	    }

        Debug.Log(data.GetHost());

        
       if(data.GetHost()) {

        NetworkManager.Singleton.StartHost();
        }
        else {NetworkManager.Singleton.StartClient();}
      
    }

   

}


