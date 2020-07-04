using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class MoveableObject : MonoBehaviour
{
    [SerializeField] private float lethalThreshold;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Knockback(Transform source, float force)
    {
        Vector3 direction = (transform.position - source.position);
        rigidBody.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        float speed = collision.relativeVelocity.magnitude;

        if(speed > lethalThreshold)
        {
            if(obj.tag == Tags.ENEMY)
                obj.GetComponent<Enemy>().Damage(speed);
            else if(obj.tag == Tags.PLAYER)
                obj.GetComponent<Player>().Damage(speed);
        }
    }
}
