using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotAnim : MonoBehaviour
{
    [SerializeField] private GameObject slingshot;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject back;

    private bool inHand = false;

    void Start()
    {
        ReturnToBack();
    }

    void Update()
    {
        if(inHand)
        {
            slingshot.transform.position = hand.transform.position;
        }
    }

    public void PassToHand()
    {
        inHand = true;
        slingshot.SetActive(true);
    }

    public void ReturnToBack()
    {
        inHand = false;
        slingshot.transform.position = back.transform.position;
        slingshot.SetActive(false);
    }
}
