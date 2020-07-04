using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEntry : KeyboardValues
{
    public CharType charType;
    public Vowel vowel;

    public void Init (CharType charType, Vowel vowel)
    {
        this.charType = charType;
        this.vowel = vowel;
    }
}
