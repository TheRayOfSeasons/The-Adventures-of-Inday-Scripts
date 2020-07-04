using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioToggler : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(transform.parent.GetComponent<Enemy>())
            {
                transform.parent.GetComponent<Enemy>().audioIsToggled = true;
                if(transform.parent.GetComponent<AudioSource>())
                    transform.parent.GetComponent<AudioSource>().Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(transform.parent.GetComponent<Enemy>())
            {
                transform.parent.GetComponent<Enemy>().audioIsToggled = false;

                if(transform.parent.GetComponent<AudioSource>())
                    transform.parent.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
