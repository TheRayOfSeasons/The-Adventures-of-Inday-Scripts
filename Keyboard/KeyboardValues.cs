using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardValues : MonoBehaviour
{
    public enum CharType
	{
		B, K, D, G, H, L, M, N, NG, P, S, T, W, Y, NoConsonant 
	}

	public enum Vowel
	{
		A, E, O, none
	}

	public CharType[] charTypeArray =
		{
			CharType.B, 
			CharType.K, 
			CharType.D, 
			CharType.G, 
			CharType.H, 
			CharType.L, 
			CharType.M, 
			CharType.N, 
			CharType.NG, 
			CharType.P, 
			CharType.S, 
			CharType.T, 
			CharType.W, 
			CharType.Y,
			CharType.NoConsonant
		};

	public Vowel[] vowelArray =
		{
			Vowel.A,
			Vowel.E,
			Vowel.O,
			Vowel.none
		};

	public string[] consonants = 
	{
		"b", 
		"k",
		"d",
		"g",
		"h",
		"l",
		"m",
		"n",
		"ng",
		"p",
		"s",
		"t",
		"w",
		"y",
		""
	};

	public string[] vowels =
	{
		"a",
		"e",
		"o",
		""
	};

	public bool IsCharacterUnlocked(CharType type)
	{
		UnlockedCharacters unlocked = GameManager.Instance.KeyboardCharacters;
		int i = (int) type;

		if(unlocked.ToArray()[i])
			return true;

		return false;
	}
}
