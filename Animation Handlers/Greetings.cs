using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greetings : MonoBehaviour
{
    [SerializeField] private string animationName;
    private Animator animator;

    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            animator.ResetTrigger(animationName);
            animator.SetTrigger(animationName);
        }   
    }
}
