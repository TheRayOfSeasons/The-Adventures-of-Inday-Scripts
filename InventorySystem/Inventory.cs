using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory", order = 5)]
public class Inventory : ScriptableObject
{
    public List<Item> items;
}
