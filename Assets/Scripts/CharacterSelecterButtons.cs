using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecterButtons : MonoBehaviour
{
    public Button nextButton;
    public Button pastButton;
    public Button selectButton;
    public Vector3 startPosition;
    public Vector3 slutPosition;
    public float positionOfSelectingCharacter;
    public float positionOfNextOrPastCharacter;
    public GameObject characterSelectorMovementContainer;
    public int i;
    public float lerpAmmount = 0.75f;
    float t;
    bool rightClick = false;
    bool leftClick = false;

    
    private void Start()
    {
        nextButton.onClick.AddListener(NextOnClick);
        pastButton.onClick.AddListener(PastOnClick);
        positionOfSelectingCharacter = 0;
        i = 1;
    }

    private void Update()
    {
        positionOfNextOrPastCharacter = 1.5f;
        startPosition = characterSelectorMovementContainer.transform.position;
        if (rightClick == true)
        {
            RightClick();
        }
        if (leftClick == true)
        {
            LeftClick();
        }
        Debug.Log(i);


    }

    void NextOnClick()
    {

        slutPosition = characterSelectorMovementContainer.transform.position - new Vector3(positionOfNextOrPastCharacter, 0, 0);

        if(i >= 2)
        {
            i = 2;
        }
        else
        {
            rightClick = true;
            i += 1;
            Debug.Log("next i");
        }

    }
    void PastOnClick()
    {
        slutPosition = characterSelectorMovementContainer.transform.position + new Vector3(positionOfNextOrPastCharacter, 0, 0);
        if (i <= 0)
        {
            i = 0;
        }
        else
        {
            leftClick = true;
            Debug.Log("past i");
            i -= 1;
            return;
        }
    }

    void RightClick()
    {

        if (slutPosition == characterSelectorMovementContainer.transform.position)
        {
            rightClick = false;
        }
        else
        {
            characterSelectorMovementContainer.transform.position = Vector3.Lerp(startPosition, slutPosition, t);
            t = Mathf.Lerp(t, .75f, lerpAmmount);
        }
    }

    void LeftClick()
    {

        if (slutPosition == characterSelectorMovementContainer.transform.position)
        {
            leftClick = false;
        }
        else
        {
            characterSelectorMovementContainer.transform.position = Vector3.Lerp(startPosition, slutPosition, t);
            t = Mathf.Lerp(t, .75f, lerpAmmount);
        }
    }
}
