using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class BaybayinArchiveButton : MonoBehaviour
{
    [SerializeField] private BaybayinData data;

    private Button button;

    void Start()
    {
        SetListener();
    }

	public void SetListener()
	{
		button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => 
            UIManager
            .Instance
            .ViewBaybayinCharacter(
                data.image, 
                data.romanized, 
                data.description,
                data.notes
            )
        );
	}

    void Update()
	{
		if (UIManager.Instance.BaybayinArchive.activeSelf)
		{
			switch(data.charType)
			{
				case Keyboard.CharType.B: 
					button.interactable = GameManager.Instance.KeyboardCharacters.b;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.b);
					break; 	
				case Keyboard.CharType.K: 
					button.interactable = GameManager.Instance.KeyboardCharacters.k;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.k);
					break; 
				case Keyboard.CharType.D: 
					button.interactable = GameManager.Instance.KeyboardCharacters.d;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.d);
					break; 
				case Keyboard.CharType.G: 
					button.interactable = GameManager.Instance.KeyboardCharacters.g;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.g);
					break; 
				case Keyboard.CharType.H: 
					button.interactable = GameManager.Instance.KeyboardCharacters.h;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.h);
					break; 
				case Keyboard.CharType.L: 
					button.interactable = GameManager.Instance.KeyboardCharacters.l;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.l);
					break; 
				case Keyboard.CharType.M: 
					button.interactable = GameManager.Instance.KeyboardCharacters.m;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.m);
					break; 
				case Keyboard.CharType.N: 
					button.interactable = GameManager.Instance.KeyboardCharacters.n;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.n);
					break; 
				case Keyboard.CharType.NG: 
					button.interactable = GameManager.Instance.KeyboardCharacters.ng;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.ng);
					break; 
				case Keyboard.CharType.P: 
					button.interactable = GameManager.Instance.KeyboardCharacters.p;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.p);
					break; 
				case Keyboard.CharType.S: 
					button.interactable = GameManager.Instance.KeyboardCharacters.s;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.s);
					break; 
				case Keyboard.CharType.T: 
					button.interactable = GameManager.Instance.KeyboardCharacters.t;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.t);
					break; 
				case Keyboard.CharType.W: 
					button.interactable = GameManager.Instance.KeyboardCharacters.w;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.w);
					break; 
				case Keyboard.CharType.Y: 
					button.interactable = GameManager.Instance.KeyboardCharacters.y;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.y);
					break;
				default:
					button.interactable = GameManager.Instance.KeyboardCharacters.vowels;
					transform.GetChild(2).gameObject.SetActive(!GameManager.Instance.KeyboardCharacters.vowels);
					break;
			}
		}
	}
}
