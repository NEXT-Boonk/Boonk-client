using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System; 



public class IpInput : MonoBehaviour
{
    
    [SerializeField]private TMP_InputField ip; // makes a field in the Unity editor for the Input field(text mesh pro)
    [SerializeField]private TMP_InputField port;
    UnityTransport UT;
    

    void Awake()
    {
        UT = GetComponent<UnityTransport>(); // finds the UnityTransport component
        port.contentType = TMP_InputField.ContentType.IntegerNumber; // makes the content of the input feild only allow Integer numbers
        port.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric; // makes the port only accept Alphanumeric values
        

    }
        
    public void InputIp(){

        UT.ConnectionData.Address = ip.text; //sets the Ip to the string in the input field
        
    }
    
    public void InputPort(){
   
        UT.ConnectionData.Port = UInt16.Parse(port.text); //sets the Port to a 5 digit number        
    }
}
