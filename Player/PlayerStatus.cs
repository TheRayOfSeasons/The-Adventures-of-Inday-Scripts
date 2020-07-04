using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "PlayerStatus", order = 3)]
public class PlayerStatus : ScriptableObject
{
    public int healthPotions;
    public int manaPotions;
}
