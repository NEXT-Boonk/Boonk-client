using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using System; 
using System.Linq;
using System.Net.Sockets;



public class ServerSelector : MonoBehaviour
{
    TextField ipInput;
    Button joinButton;
    Button hostButton;
    Button backButton;

    UnityTransport UT;
    Data data;


    string[] input;

    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        ipInput = root.Q<TextField>("IP_input");
        joinButton = root.Q<Button>("join");
        hostButton = root.Q<Button>("host");
        backButton = root.Q<Button>("back");

        joinButton.clicked += JoinButton;
        hostButton.clicked += HostButton;
        backButton.clicked += BackButton;
    }

    void JoinButton() { 

        data = FindObjectOfType<Data>();

        input = ipInput.text.Split(":");
        //string ip = Char.ToString(input[0]);

        Debug.Log(input[0]);
        Debug.Log(input[1]);
        data.SetIp(input[0]);
        data.SetPort(input[1]);
        data.SetHost(false);
        SceneManager.LoadScene("Game");

    }

    void HostButton() { 

        data = FindObjectOfType<Data>();

        input = ipInput.text.Split(":");
        //string ip = Char.ToString(input[0]);

        Debug.Log(input[0]);
        Debug.Log(input[1]);
        data.SetIp(input[0]);
        data.SetPort(input[1]);
        data.SetHost(true);
        SceneManager.LoadScene("Game");

    }

    void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
