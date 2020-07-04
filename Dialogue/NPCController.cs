using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            Vector3 player = new Vector3(
                GameManager.Instance.Player.transform.position.x,
                transform.position.y,
                GameManager.Instance.Player.transform.position.z
            );
            transform.LookAt(player);
        }
    }
}
