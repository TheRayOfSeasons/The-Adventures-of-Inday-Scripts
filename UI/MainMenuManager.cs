using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudio;

    void Start()
    {
        mainAudio.Play();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
