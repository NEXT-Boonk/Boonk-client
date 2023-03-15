using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Startmenu : MonoBehaviour 
{
    public Button startButton;
    public Button settingButton;
    public Button quitButton;

    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        settingButton = root.Q<Button>("SettingButton");
        quitButton = root.Q<Button>("QuitButton");

        startButton.clicked += StartButtonPressed;
        settingButton.clicked += SettingButtonPressed;
        quitButton.clicked += QuitButtonPressed;
    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("ServerSelector");
    }

     void SettingButtonPressed()
    {
        SceneManager.LoadScene("Settings");
    }
     
    void QuitButtonPressed()
    {
        Application.Quit();
    }
}
