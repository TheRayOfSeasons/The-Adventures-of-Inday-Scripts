using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private CameraFollow cam;
    private int obstacleCtr = 0;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.OBSTACLE)
        {
            cam.Zoom(1.5f);
            obstacleCtr++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == Tags.OBSTACLE)
        {
            obstacleCtr--;

            if(obstacleCtr <= 0)
            {
                cam.NormalDistance();
                obstacleCtr = 0;
            }
        }
    }
}
