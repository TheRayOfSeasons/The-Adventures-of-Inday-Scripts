using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingProjectile : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.ENEMY)
        {
            other.GetComponent<Enemy>().Damage(player.damage);
            Destroy(gameObject);
        }
    }
}
