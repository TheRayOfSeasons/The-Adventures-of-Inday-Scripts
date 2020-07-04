using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassAggressionTrigger : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.isAgressive = true;
            }
            gameObject.SetActive(false);
        }
    }
}
