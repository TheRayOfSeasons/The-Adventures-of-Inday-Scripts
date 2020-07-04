using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "OptionsData", order = 2)]
public class OptionsData : ScriptableObject
{
    public float MasterVolume = 100;
    public float MouseSensitivity;
    public bool EnableBaybayinKeyboardInput = true;
    public bool ShowKeyboardGuide = true;
    public bool EnableCrosshair = true;
}
