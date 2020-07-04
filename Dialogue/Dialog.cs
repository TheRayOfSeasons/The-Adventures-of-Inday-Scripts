using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public string[] messages;
    public string[] names;
    public Image[] icons;

    public Sprite defaultPortrait;
}
