using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Unlocked Spells", order = 4)]
public class UnlockedSpells : ScriptableObject
{
    public bool
        apoy,
        yelo,
        tibay,
        araw,
        galing,
        hangin;

    public bool[] GetAll()
    {
        return new bool[]
        {
            apoy, 
            yelo,
            tibay,
            araw,
            galing,
            hangin
        };
    }

    public void ResetAll()
    {
        apoy = false; 
        yelo = false;
        tibay = false;
        araw = false;
        galing = false;
        hangin = false;
    }

    public void UnlockAll()
    {
        apoy = true; 
        yelo = true;
        tibay = true;
        araw = true;
        galing = true;
        hangin = true;
    }
}
