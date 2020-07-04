using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    void Update()
    {
        Transform player = GameManager.Instance.Player.transform;

        transform.position = new Vector3 (
            player.position.x,
            transform.position.y,
            player.position.z
        );
    }
}
