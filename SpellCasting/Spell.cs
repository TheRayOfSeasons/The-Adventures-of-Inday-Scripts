using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Spell", order = 2)]
public class Spell : ScriptableObject
{
    public string Name;
    public string EnglishName;
    public string description;
    public string notes;
    public Sprite sprite;

    // points as basis for stats. Can be for Heal, Damage, etc.
    public float points;
    
    public float manaCost;

    public float duration;

    public int charges;


    // spellCode will be the required code which the player must input to cast this spell
    public string spellCode;

    // name of the spell effect's Method to be called from SpellManager 
    public SpellManager.SpellName effect;

    // prefabs of Baybayin Characters for the name
    public List<GameObject> BaybayinName;

    public bool unlocked;

    public bool isChargesType;
}
