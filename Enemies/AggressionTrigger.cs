using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressionTrigger : MonoBehaviour
{
    [SerializeField] private bool manipulateBGM;
    private Enemy enemy;

    void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(manipulateBGM)
                AudioManager.Instance.PlayCombatBGM();

            enemy.target = other.transform;
            enemy.isAgressive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(manipulateBGM)
                AudioManager.Instance.PlayCalmBGM();
        }
    }
}
