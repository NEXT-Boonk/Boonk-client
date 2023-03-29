using System; 
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Cinemachine;





public class LeaveGame : MonoBehaviour
{   
    private bool menuOpen = false;
    private Button leaveButton;
    private UnityTransport transport;
    private VisualElement root;
    [SerializeField] private Cinemachine.CinemachineFreeLook playerCamera;
    UIDocument document;

    

	void Awake() 
	{
        transport = FindObjectOfType<UnityTransport>();
        document = GetComponent<UIDocument>();
        
        root = document.rootVisualElement;
        root.style.display = DisplayStyle.None;
        

        leaveButton = root.Q<Button>("LeaveButton");
        

        leaveButton.clicked += LeaveButton;
		
	}

    private Vector2 camMaxSpeed;

    void Start(){

        camMaxSpeed = new Vector2(playerCamera.m_XAxis.m_MaxSpeed, playerCamera.m_YAxis.m_MaxSpeed);

    }

    void Update(){
        
        if (Input.GetKeyDown("escape"))
        {if (menuOpen == true){
            menuOpen = false;
            playerCamera.m_XAxis.m_MaxSpeed = camMaxSpeed.x;
            playerCamera.m_YAxis.m_MaxSpeed = camMaxSpeed.y;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            root.style.display = DisplayStyle.None;
           
           
            }
            else
            {
            menuOpen = true;
            playerCamera.m_XAxis.m_MaxSpeed = 0f;
            playerCamera.m_YAxis.m_MaxSpeed = 0f;
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
