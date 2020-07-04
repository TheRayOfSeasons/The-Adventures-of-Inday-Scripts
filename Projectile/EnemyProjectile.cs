using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.PLAYER)
        {
            other.GetComponent<Player>().Damage(Damage);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.LookAt(GameManager.Instance.Player.transform);
    }
}
