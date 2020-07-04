using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressionZone : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private bool manipulateBGM;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.isAgressive = true;
            }

            if(manipulateBGM)
                AudioManager.Instance.PlayCombatBGM();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.isAgressive = false;
            }

            if(manipulateBGM)
                AudioManager.Instance.PlayCalmBGM();
        }
    }
}
