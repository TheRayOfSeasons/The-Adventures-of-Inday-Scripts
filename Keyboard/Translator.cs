using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private Text romanized;

    void Update()
    {
        romanized.text = keyboard.GetPolishedTextValue();
    }
}
