using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject highlight;
    private Animation animation;

    void Start()
    {
        animation = transform.parent.GetComponent<Animation>();
    }

    public void Open()
    {
        animation[animation.name].speed = 1;
        animation.Play();
        highlight.SetActive(false);
    }

    public void Close()
    {
        animation[animation.name].speed = -1;
        animation[animation.name].time = animation[animation.name].length;
        animation.Play();
        highlight.SetActive(true);
    }
}
