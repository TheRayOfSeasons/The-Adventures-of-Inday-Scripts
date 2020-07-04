using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPrompter : EventManager
{
    [SerializeField] private string guide;
    [SerializeField] private string passCode;
    [SerializeField] private EventSelector eventSelector;

    void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == Tags.PLAYER)
        {
            GameManager.Instance.CurrentPasscode = passCode;
            GameManager.Instance.EventDelegate = GetDelegate(eventSelector);

            if(GameManager.Instance.KeyMode == GameManager.KeyboardMode.EventMode)
                UIManager.Instance.SetGuideText(guide);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        UIManager.Instance.ResetGuideText();
    }
}
