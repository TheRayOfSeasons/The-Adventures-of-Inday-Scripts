using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [SerializeField] private string description;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tags.PLAYER)
        {
            UIManager.Instance.SetInteractionText(description);
            UIManager.Instance.EnableInteractionText();
            UIManager.Instance.HasInteractionText = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == Tags.PLAYER)
        {
            UIManager.Instance.DisableInteractionText();
            UIManager.Instance.HasInteractionText = false;
        }
    }
}
