using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Spell Shard", order = 5)]
public class SpellShard : ScriptableObject
{
    public string Name;
    public string description;
    public EventManager.EventSelector unlock;
    public Spell spell;
}
