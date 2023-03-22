using System; 
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class LeaveGame : MonoBehaviour
{   
    private bool menuOpen = false;
    private Button leaveButton;
    private UnityTransport transport;
    private VisualElement root;
    UIDocument document;

    private string[] input;

	void Awake() 
	{
        transport = FindObjectOfType<UnityTransport>();
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        root.style.display = DisplayStyle.None;
       

        leaveButton = root.Q<Button>("LeaveButton");
        

        leaveButton.clicked += LeaveButton;
		
	}

    void Update(){
        
        if (Input.GetKeyDown("escape"))
        {if (menuOpen == true){
            menuOpen = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            root.style.display = DisplayStyle.None;
            }
            else
            {
            menuOpen = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;
            root.style.display = DisplayStyle.Flex;
            }
        }

    }

    void LeaveButton()
    { 
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("ServerSelector");
        

        
    }
    
}
