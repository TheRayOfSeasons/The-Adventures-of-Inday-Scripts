using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvents : MonoBehaviour
{
    public enum DialogEventName
    {
        ShowUIEvent
    }

    public delegate void DialogEvent();
}
