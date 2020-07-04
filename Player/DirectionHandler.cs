using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;

    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.localEulerAngles = new Vector3(0f, camera.localEulerAngles.y, 0f);
    }
}
