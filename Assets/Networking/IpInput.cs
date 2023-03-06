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
    
    //[SerializeField]private TMP_InputField ip; // makes a field in the Unity editor for the Input field(text mesh pro)
    //[SerializeField]private TMP_InputField port;
    

    string ip;
    string port;


  public void SetIp(string _Ip){

      ip = _Ip;

    }

  public void SetPort(string _Port){

      port = _Port;

    }

    public string GetIp(){

      return ip;

    }

  public string GetPort(){

      return port;

    }
    
    /*
    
       
    */

    void Awake()
    {
       // finds the UnityTransport component
       // port.contentType = TMP_InputField.ContentType.IntegerNumber; // makes the content of the input feild only allow Integer numbers
       // port.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric; // makes the port only accept Alphanumeric values

     
     

    }

    void Start(){
    /*  UT.ConnectionData.Address = ip;
      UT.ConnectionData.Port = UInt16.Parse(port);
        
        NetworkManager.Singleton.StartHost();
*/
    }
   
}
