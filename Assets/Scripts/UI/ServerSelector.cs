using System; 
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class ServerSelector : MonoBehaviour
{
    private TextField ipInput;
    private Button joinButton;
    private Button hostButton;
    private Button backButton;
    private UnityTransport transport;
    private string[] input;

	void Awake() 
	{
        transport = FindObjectOfType<UnityTransport>();

        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        ipInput = root.Q<TextField>("InputIP");
        joinButton = root.Q<Button>("JoinButton");
        hostButton = root.Q<Button>("HostButton");
        backButton = root.Q<Button>("BackButton");

        joinButton.clicked += JoinButton;
		hostButton.clicked += HostButton;
        backButton.clicked += BackButton;
	}

    void JoinButton() { 
        input = ipInput.text.Split(":");

		string ip;
		ushort port;

		// Try to parse IP and Port, return if invalid
		try {
			ip = input[0];
			port = UInt16.Parse(input[1]);
		} catch (IndexOutOfRangeException) {
			Debug.Log("Invalid IP or Port");
			return;
		} catch (FormatException) {
			Debug.Log("Invalid Port");
			return;
		}

		// Set IP and Port on transport and connect as client
		transport.ConnectionData.Address = ip;
		transport.ConnectionData.Port = port;

        if (NetworkManager.Singleton.StartClient())
        {
            // Let the network change scene.
            NetworkManager
		        .Singleton
		        .SceneManager
		        .LoadScene("GameFixed", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start client.");
        }
    }

	void HostButton() {
		// Set IP and Port (0.0.0.0:7777) and start a server as host
		transport.ConnectionData.Address = "0.0.0.0";
		transport.ConnectionData.Port = 7777;

        if (NetworkManager.Singleton.StartHost())
        {
            // Let the network change scene.
            NetworkManager
		        .Singleton
		        .SceneManager
		        .LoadScene("GameFixed", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
	}

    void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
