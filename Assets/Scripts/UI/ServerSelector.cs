using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class ServerSelector : MonoBehaviour
{
    TextField ipInput;
    Button joinButton;
    Button backButton;

    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        root.Q<TextField>("ip_input");
        joinButton = root.Q<Button>("join");
        backButton = root.Q<Button>("back");

        joinButton.clicked += JoinButton;
        backButton.clicked += BackButton;
    }

    void JoinButton() { 

    }

    void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
