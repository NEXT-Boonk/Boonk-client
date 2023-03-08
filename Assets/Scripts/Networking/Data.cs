using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System; 



public class Data : MonoBehaviour
{
    
    //[SerializeField]private TMP_InputField ip; // makes a field in the Unity editor for the Input field(text mesh pro)
    //[SerializeField]private TMP_InputField port;
    

    string ip;
    string port;

    bool host;


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

    public void SetHost(bool _Host){

      host = _Host;

    }

    public bool GetHost(){

      return host;

    }
    }