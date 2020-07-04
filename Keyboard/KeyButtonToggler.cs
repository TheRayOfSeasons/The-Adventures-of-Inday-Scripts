using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class KeyButtonToggler : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject typingModeObjects;
    [SerializeField] private GameObject nonTypingModeObjects;
    [SerializeField] private KeyboardIntegrator keyboardIntegrator;
    [SerializeField] private Text buttonText;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => ToggleTypingMode());
    }

    void ToggleTypingMode()
    {
        foreach(GameObject button in buttons)
        {
            if(HasValidComponents(button))
                ToggleComponents(button);
        }

        typingModeObjects.SetActive(!typingModeObjects.activeSelf);
        nonTypingModeObjects.SetActive(!nonTypingModeObjects.activeSelf);

        if(typingModeObjects.activeSelf)
            buttonText.text = "Back to Archives";
        else
            buttonText.text = "Test Typing";

        if(Options.Instance.Data.EnableBaybayinKeyboardInput)
            keyboardIntegrator.enable = typingModeObjects.activeSelf;
        else 
            keyboardIntegrator.enable = false;
    }

    bool HasValidComponents(GameObject button)
    {
        if(!button.GetComponent<BaybayinArchiveButton>())
            return false;

        if(!button.GetComponent<KeyboardButton>())
            return false;

        return true;
    }

    void ToggleComponents(GameObject button)
    {
        bool archiveButtonActivity = button.GetComponent<BaybayinArchiveButton>().enabled;
        bool keyButtonActivity = button.GetComponent<KeyboardButton>().enabled;
        button.GetComponent<BaybayinArchiveButton>().enabled = !archiveButtonActivity;
        button.GetComponent<KeyboardButton>().enabled = !keyButtonActivity;

        if(button.GetComponent<BaybayinArchiveButton>().enabled)
        {
            button.GetComponent<BaybayinArchiveButton>().SetListener();
            return;
        }

        if(button.GetComponent<KeyboardButton>().enabled)
        {
            button.GetComponent<KeyboardButton>().SetListener();
            return;
        }
    }
}
