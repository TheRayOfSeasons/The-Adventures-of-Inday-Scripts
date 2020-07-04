using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastPointRadius : MonoBehaviour
{
    private float time,
                  points;

    private Player player;

    void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
    }

    public void SetPoints(float time, float dps)
    {
        this.time = time;
        this.points = dps;
    }

    public void SetPoints(float points)
    {
        this.points = points;
    }

    void OnTriggerStay(Collider other)
    {
        if(player.isRadianceOn)
        {
            if(other.tag == Tags.ENEMY)
            {
                other.GetComponent<Enemy>().Burn(time, points);
            }
        }

        if(player.canPush)
        {
            if(other.tag == Tags.MOVEABLE)
                other.GetComponent<MoveableObject>().Knockback(transform, points);
            else if(other.tag == Tags.ENEMY)
                other.GetComponent<Enemy>().Knockback(time, points);
        }
    }
}
