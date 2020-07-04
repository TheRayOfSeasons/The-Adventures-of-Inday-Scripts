using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButton : KeyboardValues
{
	private Button button;

	[SerializeField] private Keyboard keyboard;
	[SerializeField] private Keyboard.CharType charType;
	[SerializeField] private Keyboard.Vowel vowel;

	private enum ButtonType
	{
		Input,
		Modify,
		Space,
		Backspace
	}

	[SerializeField] private ButtonType buttonType;

	void Start ()
	{
		SetListener();
	}

	public void SetListener()
	{
		button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		switch (buttonType)
		{
			case ButtonType.Input:
				button.onClick.AddListener(() => keyboard.TypeBaybayin(charType, vowel)); 
				break;
			case ButtonType.Modify:	
				button.onClick.AddListener(() => keyboard.ModifyBaybayin(vowel)); 
				break;
			case ButtonType.Space:
				break;
			case ButtonType.Backspace:
				break;
		}
	}

	void Update()
	{
		if(GameManager.Instance.baybayinKeyboard.activeSelf || UIManager.Instance.BaybayinArchive.activeSelf)
		{
			switch(charType)
			{
				case CharType.B: 
					button.interactable = GameManager.Instance.KeyboardCharacters.b;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.b);
					break; 
				case CharType.K: 
					button.interactable = GameManager.Instance.KeyboardCharacters.k;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.k);
					break; 
				case CharType.D: 
					button.interactable = GameManager.Instance.KeyboardCharacters.d;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.d);
					break; 
				case CharType.G: 
					button.interactable = GameManager.Instance.KeyboardCharacters.g;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.g);
					break; 
				case CharType.H: 
					button.interactable = GameManager.Instance.KeyboardCharacters.h;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.h);
					break; 
				case CharType.L: 
					button.interactable = GameManager.Instance.KeyboardCharacters.l;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.l);
					break; 
				case CharType.M: 
					button.interactable = GameManager.Instance.KeyboardCharacters.m;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.m);
					break; 
				case CharType.N: 
					button.interactable = GameManager.Instance.KeyboardCharacters.n;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.n);
					break; 
				case CharType.NG: 
					button.interactable = GameManager.Instance.KeyboardCharacters.ng;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.ng);
					break; 
				case CharType.P: 
					button.interactable = GameManager.Instance.KeyboardCharacters.p;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.p);
					break; 
				case CharType.S: 
					button.interactable = GameManager.Instance.KeyboardCharacters.s;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.s);
					break; 
				case CharType.T: 
					button.interactable = GameManager.Instance.KeyboardCharacters.t;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.t);
					break; 
				case CharType.W: 
					button.interactable = GameManager.Instance.KeyboardCharacters.w;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.w);
					break; 
				case CharType.Y: 
					button.interactable = GameManager.Instance.KeyboardCharacters.y;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.y);
					break;
				case CharType.NoConsonant:
					button.interactable = GameManager.Instance.KeyboardCharacters.vowels;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.vowels);
					break;
			}
		}
	}
}
