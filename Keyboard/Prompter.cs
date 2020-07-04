using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompter : MonoBehaviour 
{
	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == Tags.PLAYER)
		{
			if(Input.GetKeyUp(KeyCode.E))
            {
				GameManager.Instance.KeyMode = GameManager.KeyboardMode.EventMode;
				GameManager.Instance.ManualToggleKeyboard(true);
			}
		}
	}

	void OnTriggerExit (Collider collider)
	{
		if (collider.tag == Tags.PLAYER)
		{
			GameManager.Instance.ManualToggleKeyboard(false);
		}
	}
}
