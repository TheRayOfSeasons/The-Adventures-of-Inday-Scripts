using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Baybayin Data", order = 8)]
public class BaybayinData : ScriptableObject
{
    public Keyboard.CharType charType;
    public Image image;
    public string romanized;
    public string description;
    public string notes;
}
