using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public enum ActionSelector
    {
        Charge,
        PrintY
    }

    public delegate void Action(GameObject enemyObject, Enemy enemy);
    public static Action[] Actions =
    {
        Charge,
        PrintY
    };

    public static void Do(ActionSelector selectedAction, GameObject enemyObject, Enemy enemy)
    {
        Actions[(int)selectedAction](enemyObject, enemy);
    }

    public static void Charge(GameObject enemyObject, Enemy enemy)
    {
        if(enemy.Mob == Enemy.MobType.Tikbalang)
        {
            enemy.gameObject.GetComponent<Animator>().SetBool("Charging", true);
            enemy.Boost(30f, 3f);
            return;
        }

        else if(enemy.Mob == Enemy.MobType.Tornado)
            enemy.Boost(50f, 3f);
    }

    public static void PrintY(GameObject enemyObject, Enemy enemy)
    {
        Debug.Log("Y");
    }
}
