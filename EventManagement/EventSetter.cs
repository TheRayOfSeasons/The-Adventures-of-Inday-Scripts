using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(GetComponent<Door>())
                EventManager.currentDoor = GetComponent<Door>();
            if(transform.parent.GetComponent<Animator>())
                EventManager.currentShopAnimator = transform.parent.GetComponent<Animator>();
        }
    }
}
