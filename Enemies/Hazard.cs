using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damage;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
            other.GetComponent<Player>().Damage(damage);
    }
}
