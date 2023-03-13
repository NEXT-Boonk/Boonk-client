using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using Unity.Netcode.Transports.UTP;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using System; 
using System.Linq;
using System.Net.Sockets;



public class ServerJoin : MonoBehaviour
{

    public TextField ipPort;
    public Button joinButton;
    public Button hostButton;


    UnityTransport UT;
    IpInput iP;

    string[] input;

    

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        ipPort = root.Q<TextField>( "IpPort" );
        joinButton = root.Q<Button>("JoinButton");
        hostButton = root.Q<Button>("HostButton");

        joinButton.clicked += JoinButtonPressed;
        hostButton.clicked += HostButtonPressed;
        
    }

    void JoinButtonPressed()
    {

        if(ipPort.text == "Ip..." || ipPort.text == "" ){
            return;
        }
        iP = FindObjectOfType<IpInput>();

        input = ipPort.text.Split(":");
        //string ip = Char.ToString(input[0]);

        Debug.Log(input[0]);
        Debug.Log(input[1]);
        iP.SetIp(input[0]);
        iP.SetPort(input[1]);
        iP.SetHost(false);
        SceneManager.LoadScene("Game");

       }

       void HostButtonPressed()
    {

        if(ipPort.text == "Ip..." || ipPort.text == "" ){
            return;
        }
        iP = FindObjectOfType<IpInput>();

        input = ipPort.text.Split(":");
        //string ip = Char.ToString(input[0]);

        Debug.Log(input[0]);
        Debug.Log(input[1]);
        iP.SetIp(input[0]);
        iP.SetPort(input[1]);
        iP.SetHost(true);
        SceneManager.LoadScene("Game");

       }

        

        //UT.ConnectionData.Address = input[0];
        //UT.ConnectionData.Port = UInt16.Parse(input[1]);
        

        
       
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
