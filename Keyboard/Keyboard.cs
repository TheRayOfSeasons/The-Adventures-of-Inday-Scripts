using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : KeyboardValues 
{
	[SerializeField] private string textValue = "";
	public string TextValue
	{
		get { return textValue; }
	}

	[SerializeField] private List<List<GameObject>> characters;
	[SerializeField] private GameObject 
		a, e, o;
	[SerializeField] private List<GameObject> 
		b, k, d, g, h, l, m, n, ng, p, s, t, w, y, noConsonant;

	[SerializeField] private List<Button> buttons;
	[SerializeField] private GameObject 
		_canvas,
		inputOrigin;

	[SerializeField] private float padding = 50f;
	private float newTextPosition = 0f; // reset

	private Vector3 permpos;

	private GameObject previousEntry;
	private List<CharType> previousCharTypes;
	private List<Vowel> previousVowels;
	private List<GameObject> previousList;

	[SerializeField] Button[] modifierButtons;
	[SerializeField] Button backspaceButton;
	[SerializeField] List<GameObject> charEntries;

	[SerializeField] private int characterLimit;

	void Start () 
	{
		//initialize lists of character type lists
 		characters = new List<List<GameObject>>
		 {
			 b, k, d, g, h, l, m, n, ng, p, s, t, w, y, noConsonant
		 };

		 previousCharTypes = new List<CharType>();
		 previousVowels = new List<Vowel>();
		//establish constant variable for input origin position
		 permpos = inputOrigin.transform.position;

		 //Start with some buttons toggled off
		 ToggleButtons(modifierButtons, false);
		 ToggleButton(backspaceButton, false);
	}

	public void TypeBaybayin (CharType charType, Vowel vowel)
	{
		if (charEntries.Count >= characterLimit)
			return;

		if(charType != CharType.NoConsonant)
			if(!IsCharacterUnlocked(charType))
				return;
			
		//adjust existing character inputs to fit next
		AddCharacterAdjustment();

		//instantiate new character input
		GameObject characterToInput = Instantiate(GetCharacter(charType, vowel));
		characterToInput.transform.SetParent(inputOrigin.transform);
		characterToInput.transform.position = new Vector3
		(
			inputOrigin.transform.position.x + newTextPosition,
			inputOrigin.transform.position.y,
			inputOrigin.transform.position.z
		);

		InitCharEntry(characterToInput, charType, vowel);
		charEntries.Add(characterToInput);

		//keep characters centered
		inputOrigin.transform.position = permpos;
		
		// set new position of typing cursor
		newTextPosition += padding;
		
		updateValue ();

		HandleButtons();
	}

	// modifies the vowel in the character
	public void ModifyBaybayin (Vowel vowel)
	{
		GameObject lastElement = GetLastElement(charEntries);
		CharType charType = lastElement.GetComponent<CharEntry>().charType;
	
		if(charType == CharType.NoConsonant)
			return;
			
		GameObject characterToInput = Instantiate(GetCharacter(charType, vowel));

		characterToInput.transform.SetParent(inputOrigin.transform);
		characterToInput.transform.position = lastElement.transform.position;
		InitCharEntry(characterToInput, charType, vowel);
		charEntries.Remove(lastElement);
		Destroy(lastElement);
		charEntries.Add(characterToInput);
		updateValue();
	}

	public void BackSpace ()
	{
		RectTransform[] children = inputOrigin.GetComponentsInChildren<RectTransform>();
		GameObject lastEntry = GetLastElement(charEntries);
		CharType lastConsonant = GetLastElement(charEntries).GetComponent<CharEntry>().charType;
		Vowel lastVowel = GetLastElement(charEntries).GetComponent<CharEntry>().vowel;

		if ((lastConsonant != CharType.NG) && (lastVowel == Vowel.none))
		{
			textValue = textValue.Remove(textValue.Length - 1);
		}
		else if ((lastConsonant == CharType.NG) && (lastVowel != Vowel.none))
		{
			textValue = textValue.Remove(textValue.Length - 3);
		}
		else
		{
			textValue = textValue.Remove(textValue.Length - 2);
		}

		inputOrigin.transform.position = permpos;
		charEntries.Remove(lastEntry);
		Destroy(lastEntry);

		RemovePreviousCharacter();
		newTextPosition -= padding;

		HandleButtons();
	}

	public void Clear()
	{
		reset();
	}

	private void reset()
	{
		textValue = "";
		newTextPosition = 0f;
		previousEntry = null;
		previousCharTypes = new List<CharType>();
		previousVowels = new List<Vowel>();
		previousList = null;
		foreach (GameObject go in charEntries)
		{
			Destroy(go);
		}
		charEntries = new List<GameObject>();
	}

	private void updateValue ()
	{
		string accumulator = "";

		for (int i = 0; i < charEntries.Count; i++)
		{
			for (int j = 0; j < charTypeArray.Length; j++)
			{
				if (charEntries[i].GetComponent<CharEntry>().charType == charTypeArray[j])
					accumulator += consonants[j];
			}

			for (int j = 0; j < vowelArray.Length; j++)
			{
				if (charEntries[i].GetComponent<CharEntry>().vowel == vowelArray[j])
					accumulator += vowels[j];
			}
		}

		textValue = accumulator;
	}

	private void InitCharEntry (GameObject entry, CharType charType, Vowel vowel)
	{
		entry.AddComponent<CharEntry>();
		entry.GetComponent<CharEntry>().Init(charType, vowel);
	}

	private T GetLastElement<T> (List<T> list)
	{
		return list[list.Count - 1];
	}

	private bool IsOdd (float num)
	{
		if (num%2==0)
			return false;
		if (num%2==1)
			return true;

		return false;
	}

	private GameObject GetCharacter (CharType charType, Vowel vowel)
	{
		//List that will contain types of the selected character
		List<GameObject> selected = new List<GameObject>();

		//Transfer list of types to selected
		for (int i = 0; i < charTypeArray.Length; i++)
		{
			if (charType == charTypeArray[i])
			{
				selected = characters[i];
				break;
			}
		}

		//Return character with the specified vowel
		for (int i = 0; i < vowelArray.Length; i++)
		{
			if (vowel == vowelArray[i])
			{
				return selected[i];
			}
		}

		return null;
	}

	private void AddCharacterAdjustment ()
	{
		RectTransform[] children = inputOrigin.GetComponentsInChildren<RectTransform>();

		// previous version
		if (children.Length > 1)
		{
			// //adjusts all previously inputted characters
			foreach (Transform i in children)
			{
				if (i != inputOrigin)
				{
					i.position = new Vector3
					(
						i.position.x - padding,
						i.position.y,
						i.position.z
					);
				}
			}
		}
	}

	private void RemovePreviousCharacter ()
	{
		RectTransform[] children = inputOrigin.GetComponentsInChildren<RectTransform>();

		if (children.Length > 1)
		{
			//adjusts all previously inputted characters
			foreach (Transform i in children)
			{
				if (i != inputOrigin)
				{
					i.position = new Vector3
					(
						i.position.x + padding,
						i.position.y,
						i.position.z
					);
				}
			}
		}

		inputOrigin.transform.position = permpos;
	}

	private void HandleButtons ()
	{
		if (charEntries.Count > 0)
		{
			if (GetLastElement(charEntries).GetComponent<CharEntry>().charType != CharType.NoConsonant)
			{
				ToggleButtons(modifierButtons, true);
			}
			else
				ToggleButtons(modifierButtons, false);

			ToggleButton(backspaceButton, true);
		}
		else
		{
			ToggleButton(backspaceButton, false);
			ToggleButtons(modifierButtons, false);
		}
	}

	private void ToggleButton (Button button, bool toggle)
	{
		button.interactable = toggle;
	}

	private void ToggleButtons (Button[] buttons, bool toggle)
	{
		foreach (Button button in buttons)
			button.interactable = toggle;
	}

	public string GetPolishedTextValue()
	{
		return textValue.ToUpper();
	}
}