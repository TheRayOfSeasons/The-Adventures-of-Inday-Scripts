using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardIntegrator : KeyboardValues
{
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private GameObject[] KeyGuides;
    [SerializeField] private GameObject keyboardPanel;
    public bool enable;
    private bool doOnce = true;
    private bool differentiator;

    private KeyCode[] vowelKeys = 
    {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E
    };

    private KeyCode[] consonantKeys = 
    {
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y,
        KeyCode.U,
        KeyCode.I,
        KeyCode.O,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.J,
        KeyCode.K
    };

    private KeyCode[] modifierKeys =
    {
        KeyCode.C,
        KeyCode.V,
        KeyCode.B
    };

    void Start()
    {
        EnableKeyboardInput(enable);
        differentiator = Options.Instance.Data.ShowKeyboardGuide;
    }

    void Update()
    {       
        if(!enable)
            return;

        if(!Options.Instance.Data.EnableBaybayinKeyboardInput)
            return;

        if(!keyboardPanel.activeSelf)
            return;

        if(DialogManager.Instance.isDialogRunning)
            return;

        for (int i = 0; i < vowelKeys.Length; i++)
        {
            if(Input.GetKeyDown(vowelKeys[i]))
                keyboard.TypeBaybayin(CharType.NoConsonant, vowelArray[i]);
        }

        for(int i = 0; i < consonantKeys.Length; i++)
        {
            if(Input.GetKeyDown(consonantKeys[i]))
                keyboard.TypeBaybayin(charTypeArray[i], Vowel.A);
        }

        for (int i = 0; i < modifierKeys.Length; i++)
        {
            if(Input.GetKeyDown(modifierKeys[i]))
                keyboard.ModifyBaybayin(vowelArray[i + 1]);
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
            keyboard.BackSpace();
        
        if(Input.GetKeyDown(KeyCode.Pipe))
            keyboard.Clear();
    }

    void FixedUpdate()
    {
        if(Options.Instance.Data.ShowKeyboardGuide != differentiator)
            doOnce = true;

        if(!doOnce)
            return;
            
        ToggleKeyGuide(Options.Instance.Data.ShowKeyboardGuide);
        differentiator = Options.Instance.Data.ShowKeyboardGuide;
        doOnce = false;
    }

    public void EnableKeyboardInput(bool toggle)
    {
        ToggleKeyGuide(toggle);
        enable = toggle;
    }

    private void ToggleKeyGuide(bool toggle)
    {
        foreach(GameObject guide in KeyGuides)
            guide.SetActive(toggle);
    }
}
