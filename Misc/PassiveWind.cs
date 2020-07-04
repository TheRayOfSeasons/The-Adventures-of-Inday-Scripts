using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveWind : MonoBehaviour
{
    [SerializeField] private float windSpeed;

    void OnTriggerStay(Collider other)
    {
        if(other.tag == Tags.MOVEABLE)
            other.GetComponent<MoveableObject>().Knockback(transform, windSpeed);
    }
}
