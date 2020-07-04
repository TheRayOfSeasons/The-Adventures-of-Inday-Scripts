using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Item", order = 4)]
public class Item : ScriptableObject
{
    public string id;
    public ItemManager.ItemNames Name;
    public string description;
    public Sprite sprite;
    public bool Stackable;
    public bool Useable;
    public bool isSpellShard;
    public bool isBuff;
    public SpellShard spellShard;
    public int points;
    public int Stacks;
    public int BuyPrice;
    public int SellPrice;
}
