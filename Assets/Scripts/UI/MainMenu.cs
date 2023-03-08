using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenu : MonoBehaviour
{
    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        Button startButton = root.Q<Button>("start");
        Button quitButton = root.Q<Button>("quit");

        startButton.clicked += StartButton;
        quitButton.clicked += QuitButton;
    }

    void StartButton() { 
        SceneManager.LoadScene("ServerSelector");   
    }

    void QuitButton() {
        Application.Quit();
    }
}
