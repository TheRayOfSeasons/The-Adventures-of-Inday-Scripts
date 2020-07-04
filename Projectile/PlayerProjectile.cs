using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private Spell stats;
    [SerializeField] private AudioSource hurlingAudio;
    public Effect effect;
    
    public float BonusDamage
    {
        get { return bonusDamage; }
        set { bonusDamage = value; }
    }
    private static float bonusDamage = 0f;

    public enum Effect
    {
        PlainDamage,
        Burn,
        Slow
    }

    private delegate void ProjectileEffect(GameObject affected);
    private ProjectileEffect[] projectileEffects =
    {
        PlainDamage,
        Burn,
        Slow
    };

    private static Spell staticStats;

    void Start()
    {
        staticStats = stats;

        if(hurlingAudio)
            hurlingAudio.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.ENEMY)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.isAgressive = true;
            DoEffect(effect, other.gameObject);
            Destroy(gameObject);
        }
    }

    private void DoEffect(Effect effect, GameObject affected)
    {
        projectileEffects[(int) effect](affected);
    }

    private static void PlainDamage(GameObject affected)
    {
        affected.GetComponent<Enemy>().Damage(
            GameManager.Instance.Player.GetComponent<Player>().damage + bonusDamage
        );
    }

    private static void Burn(GameObject affected)
    {
        Enemy enemy = affected.GetComponent<Enemy>();
        PlainDamage(affected);

        enemy.Burn(staticStats.points, staticStats.duration);
    }

    private static void Slow(GameObject affected)
    {
        Enemy enemy = affected.GetComponent<Enemy>();
        PlainDamage(affected);

        enemy.Slow(staticStats.points, staticStats.duration);
    }
}
