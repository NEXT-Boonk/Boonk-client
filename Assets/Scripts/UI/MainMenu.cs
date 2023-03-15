using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenu : MonoBehaviour
{
    private Button playButton;
    private Button quitButton;

    void Awake()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        playButton = root.Q<Button>("PlayButton");
        quitButton = root.Q<Button>("QuitButton");

        playButton.clicked += PlayButton;
        quitButton.clicked += QuitButton;
    }

    void PlayButton()
    { 
        SceneManager.LoadScene("ServerSelector");   
    }

    void QuitButton()
    {
        Application.Quit();
    }
}
