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


public class NetworkManagerUI : MonoBehaviour
{
    // makes a field in the Unity editor for the Buttons
    [SerializeField]private Button server;
    [SerializeField]private Button host;
    [SerializeField]private Button client;
    [SerializeField]private Button disconnect;

    // Deifining Unity transport
    UnityTransport UT;

    string ip;
    string port;



    //UnityTransport UT;

    private void Awake(){

        
        //UT = FindObjectOfType<UnityTransport>(); // finds the object UnityTransport 
        //UT.ConnectionData.Address = "127.0.0.1";

        //UT.ConnectionData.Port = UInt16.Parse(port);
        //UT.ConnectionData.Address = ip;

        
        
        // Made by dantheman213
        // https://gist.github.com/dantheman213/db3118bed76199186acf7be87af0c1c4
        // searches after an ip from an avalibe Wi-fi eller Ethernet
        /*
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

		foreach (NetworkInterface adapter in interfaces.Where(x => x.OperationalStatus == OperationalStatus.Up)) {
			if (adapter.Name.ToLower() == "ethernet" || adapter.Name.ToLower() == "wi-fi") {

			    ipInterfaceProperties props = adapter.GetIpProperties();
			    UnicastipAddressInformation result = props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork);

			    if (result != null) {
				    ip = result.Address.ToString();
			    }
			}
	    }*/
    
        
        
        string[] args = System.Environment.GetCommandLineArgs();

        for(int i = 0; i < args.Length; i++) {
            
            if(args[i] == "--launch-as-client") { // runs the game as a client 
                NetworkManager.Singleton.StartClient();
            }
             /*
            runs the game as a server
            with the ip of the local nertwork and a hard coded port
            */
            else if(args[i] == "--launch-as-server") { 
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartServer();
            }
            else if(args[i] == "--launch-as-host") { //køre programmet som en host med local ip'en af det netværk systemet er forbundet til med porten 60000
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartHost();
            }
        }


      
    }

    void Update(){
        // prints the ip and port in the console
        //Debug.Log("ip:" + UT.ConnectionData.Address + " port:" + UT.ConnectionData.port);
    }

}


