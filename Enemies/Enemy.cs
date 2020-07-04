using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

//comment this out on build
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float roamRadius = 5;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool pushable;
    [SerializeField] private bool immortal;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject aggressionTrigger;
    [SerializeField] private GameObject[] deactivateOnDeath;
    [SerializeField] private bool independentBGM;

    public string Name;
    public float hitPoints;
    public float maxHitPoints;
    public float damage;

    public bool isAgressive;
    private bool currentAggression;

    private GameObject player;
    private NavMeshAgent agent;
    private Rigidbody rigidbody;
    private Vector3 originPosition;
    private float speed;
    [SerializeField] private Animator animator;

    public Transform target;
    private int currentWaypoint = -1;

    // Attack Trigger related
    [SerializeField] private float atkSpeed = 2f;
    private float atkTimer = 0f;
    private bool canAttackPlayer = true;
    private bool isDead = false;
    private float deactivateTimer = 1f;

    public enum Movement
    {
        Patrol,
        Roam,
        Idle
    };
    public Movement movement;

    public enum MobType
    {
        Simple,
        Ranged,
        Tikbalang,
        Boss,
        Tornado
    };
    [SerializeField] private MobType mobType;
    public MobType Mob 
    {
        get { return mobType; }
    }
    [SerializeField] private List<GameObject> drops;
    [SerializeField] private List<GameObject> chanceDrops;
    [SerializeField] private float chance;
    
    [Header("If Special")]
    [SerializeField] private List<EnemyActions.ActionSelector> actions;
    [SerializeField] private bool canSpecialAction;
    [SerializeField] private float specialActionTimer;
    [SerializeField] private float knockbackDamageMultiplier;
    private float specialActionCtr;

    // Enemy Status Effects
    private delegate void TimedAction(float modifier, float timer);
    private TimedAction timedAction;
    private float actionBuffTimer = 0f;
    private float modifierResetValue;
    private bool canKnockBackPlayer;

    private delegate void TimedStatus(float modifier);
    private List<TimedStatus> debuffs;
    private List<float> debuffModifiers;
    private List<float> debuffTimers;
    private float debuffResetValue;
    private bool isDebuffed = false;

    private bool isBurning = false;
    private bool isSlowed = false;

    private float pushBackTimer = 0f;
    private float pushForce = 1f;
    private bool knocked = false;

    [Header("If Ranged")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSource;
    [SerializeField] private float range;
    [SerializeField] private float projectileSpeed;

    [Header("Audio")]
    [SerializeField] private AudioSource idleAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource aggressionAudio;
    [SerializeField] private AudioSource specialActionAudio;

    [SerializeField] private bool canPlayIdleAudio;
    public bool audioIsToggled = false;

    [SerializeField] private float idleAudioTimer;

    [Header("Particle Systems")]
    [SerializeField] private GameObject burningParticles;

    private float idleAudioCounter;

    private int index = 0;
    private bool added = false;

    public void Heal(float amount)
    {
        hitPoints += amount;
    }

    public void Damage(float amount)
    {
        if(immortal)
            return;

        if(GameManager.Instance.enemiesArePaused)
            return;
            
        hitPoints -= 
            GameManager
            .Instance
            .Player
            .GetComponent<Player>()
            .isDoubleDamage ? amount * 2 : amount;

        if (hitPoints <= 0)
            Die();
    }

    public void DamagePlayer(float dmg)
    {
        if(GameManager.Instance.enemiesArePaused)
            return;
            
        GameManager.Instance.Player.GetComponent<Player>().Damage(dmg);
    }

    void Start()
    {
        ResetDebuffs();

        player = GameManager.Instance.Player;

        hitPoints = maxHitPoints;
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        originPosition = transform.position;
        speed = agent.speed;

        idleAudioCounter = idleAudioTimer;
    }

    void Update()
    {
        if(isDead)
        {
            deactivateTimer -= Time.deltaTime;
            if(deactivateTimer <= 0)
            {
                
                for(int i = 0; i < deactivateOnDeath.Length; i++)
                    deactivateOnDeath[i].SetActive(false);
            }
            
            return;
        }

        if(GameManager.Instance.IsPaused)
            return;

        if(GameManager.Instance.enemiesArePaused)
            return;

        if(idleAudio && !isAgressive && canPlayIdleAudio && audioIsToggled && !isDead)
        {
            idleAudioCounter -= Time.deltaTime;

            if(idleAudioCounter <= 0)
            {
                idleAudioCounter = Random.Range(idleAudioTimer, idleAudioTimer * 1.75f);
                idleAudio.Play();
            }
        }

        if(currentAggression != isAgressive)
        {
            marker.SetActive(isAgressive);
            aggressionTrigger.SetActive(!isAgressive);
            if(mobType == MobType.Simple)
                agent.stoppingDistance = isAgressive ? 1.1f : 0f;

            if(isAgressive)
            {
                added = true;
                if(!independentBGM)
                    index = GameManager.Instance.AddHostile(this);

                if(!independentBGM)
                    AudioManager.Instance.PlayCombatBGM();

                if(aggressionAudio && canPlayIdleAudio && !isDead)
                    aggressionAudio.Play();
            }
        }

        currentAggression = isAgressive;
        
        if(knocked)
        {
            if(pushBackTimer > 0)
            {
                pushBackTimer -= Time.deltaTime;
                PushBack();
            }
            else 
            {
                pushBackTimer = 0f;
                knocked = false;
            }
        }
        else if (isAgressive)
        {
            Attack();
        }
        else
        {
            switch (movement)
            {
                case Movement.Patrol:
                    Patrol();
                    break;
                case Movement.Roam:
                    Roam();
                    break;
                default:
                    break;
            }
        }


        if(actionBuffTimer > 0)
            actionBuffTimer -= Time.deltaTime;
        else
        {
            if(timedAction != null)
                timedAction(modifierResetValue, 0f);
        }
    }

    void FixedUpdate()
    {
        if(isDead)
        {
            deactivateTimer -= Time.deltaTime;
            if(deactivateTimer <= 0)
            {
                for(int i = 0; i < deactivateOnDeath.Length; i++)
                    deactivateOnDeath[i].SetActive(false);
            }
            
            return;
        }

        if(GameManager.Instance.enemiesArePaused)
            return;

        if(GameManager.Instance.IsPaused)
            return;
            
        rigidbody.velocity = Vector3.zero;
        if(isAgressive)
        {
            if(atkTimer > 0)
                atkTimer -= Time.fixedDeltaTime;
            else
                canAttackPlayer = true;
        }

        if(isDebuffed)
        {
            for(int i = 0; i < debuffs.Count; i++)
            {
                TimedStatus statusEffect = debuffs[i];
                float debuffModifier = debuffModifiers[i];
                float debuffTimer = debuffTimers[i];

                if (debuffTimer > 0)
                {
                    debuffTimers[i] -= Time.fixedDeltaTime;
                    statusEffect(debuffModifier);
                }
                else
                {
                    RemoveStatusEffect(statusEffect, i);
                    if(AllDebuffsFinished())
                    {
                        isDebuffed = false;
                        ResetDebuffs();
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(isDead)
            return;

        if(GameManager.Instance.enemiesArePaused)
            return;

        if(isAgressive)
        {
            if(other.gameObject.tag == "Melee Threshold")
            {
                switch(mobType)
                {
                    case MobType.Simple:
                        if(animator)
                            animator.SetBool("Attacking", true);
                        break;
                    case MobType.Tikbalang:
                        animator.SetBool("Attacking", true);
                        break;
                }

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(isDead)
            return;

        if(other.gameObject.tag == "Melee Threshold")
        {
            switch(mobType)
            {
                case MobType.Simple:
                    if(animator)
                        animator.SetBool("Attacking", false);
                    break;
                case MobType.Tikbalang:
                    animator.SetBool("Attacking", false);
                    break;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(isDead)
            return;

        if(GameManager.Instance.enemiesArePaused)
            return;

        if (collision.gameObject.tag == Tags.PLAYER)
        {
            GameObject target = collision.gameObject;

            switch(mobType)
            {
                case MobType.Simple:
                    // animator.SetBool("Attacking", true);
                    break;
                case MobType.Tikbalang:
                    if(canKnockBackPlayer)
                    {
                        Vector3 direction = (
                            target.transform.position - 
                            transform.position
                        ).normalized;
                        Vector3 knockback = new Vector3(
                            direction.x,
                            direction.y + 10f,
                            direction.z
                        );
                        target.GetComponent<Player>().Knockback(direction, 5f);
                        target.GetComponent<Player>().Damage(knockbackDamageMultiplier);

                        if(specialActionAudio);
                            specialActionAudio.Play();
                    }
                    break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == Tags.PLAYER)
        {
            if(canKnockBackPlayer)
                canKnockBackPlayer = false;
        }
    }

    void Patrol()
    {
        if(waypoints == null && waypoints.Length == 0)
            return;
        
        if(agent.remainingDistance != 0)
			return;	

		if(agent.remainingDistance == 0)
			currentWaypoint++;

		if(currentWaypoint == waypoints.Length)
			currentWaypoint = 0;

        agent.destination = waypoints[currentWaypoint].position;
    }

    void Roam()
    {
        if(agent.remainingDistance != 0)
            return;

        Vector3 randomPosition = Random.insideUnitSphere * roamRadius;
        randomPosition += originPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, roamRadius, 1);
        Vector3 newPosition = hit.position;
        agent.destination = newPosition;
    }

    void Attack()
    {
        switch(mobType)
        {
            case MobType.Tikbalang:
                TikbalangAttack();
                break;
            case MobType.Boss:
                break;
            case MobType.Ranged:
                RangedFollow();
                break;
            case MobType.Tornado:
                TornadoAttack();
                break;
            default:
                Follow();
                break;
        }
    }

    void RangedAttack()
    {
        if(GameManager.Instance.enemiesArePaused)
            return;

        atkTimer = atkSpeed;
        canAttackPlayer = false;

        GameObject prefab = Instantiate(projectile);
        projectile.transform.position = projectileSource.position;

        if (prefab.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
            Vector3 playerOffset = new Vector3(
                GameManager.Instance.Player.transform.position.x,
                GameManager.Instance.Player.transform.position.y + 1.5f,
                GameManager.Instance.Player.transform.position.z
            );
            Vector3 direction = (
                playerOffset -
                projectileSource.transform.position
            ).normalized;
            rigidbody.velocity = direction * projectileSpeed;
        }

        Destroy(prefab, 2.5f);
    }

    // Enemy Statuses
    public void Boost(float speed, float timer)
    {
        if(!GetComponent<ThirdPersonCharacter>() && mobType != MobType.Tornado)
            return;

        ThirdPersonCharacter character;
        if(GetComponent<ThirdPersonCharacter>())
        {
            character = GetComponent<ThirdPersonCharacter>();
            character.MoveSpeedMultiplier = speed;
        }

        agent.speed = speed;

        actionBuffTimer = timer;
        timedAction = Boost;
        modifierResetValue = this.speed;

        if(timer > 0)
            canKnockBackPlayer = true;
        else
        {
            canKnockBackPlayer = false;

            if(mobType == MobType.Tikbalang)
                animator.SetBool("Charging", false);
        }
    }

    public void Burn(float dps, float time)
    {
        if(!isBurning)
        {
            burningParticles.SetActive(true);
            isDebuffed = true;
            isBurning = true;
            debuffs.Add(Burning);
            debuffModifiers.Add(dps);
            debuffTimers.Add(time);
        }
    }

    private void Burning(float dps)
    {
        Damage(dps);
    }

    public void Slow(float slowAmount, float time)
    {
        if(!isSlowed)
        {
            isDebuffed = true;
            isSlowed = true;
            debuffs.Add(Iced);
            debuffModifiers.Add(slowAmount);
            debuffTimers.Add(time);
        }
    }

    public void Knockback(float time, float force)
    {
        if(!pushable)
            return;

        knocked = true;
        pushBackTimer = time;
        pushForce = force;
    }

    private void PushBack()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;
        agent.destination = direction * pushForce;
    }

    private void Iced(float slowAmount)
    {
        agent.speed = slowAmount;
    }

    private bool AllDebuffsFinished()
    {
        foreach(float i in debuffTimers)
        {
            if(i > 0)
                return false;
        }

        return true;
    }

    private void RemoveStatusEffect(TimedStatus status, int index)
    {
        Debug.Log("Removing Status Effect");
        debuffs.RemoveAt(index);
        debuffModifiers.RemoveAt(index);
        debuffTimers.RemoveAt(index);

        if(status == Burning)
        {
            isBurning = false;
            burningParticles.SetActive(false);
        }
        if(status == Iced)
        {
            Debug.Log("Removing Yelo");
            isSlowed = false;
            agent.speed = speed;
        }
    }

    private void ResetDebuffs()
    {
        debuffs = new List<TimedStatus>();
        debuffModifiers = new List<float>();
        debuffTimers = new List<float>();
    }

	private void Follow()
	{
		LookTarget();
		agent.destination = player.transform.position;
	}

    private void TikbalangFollow()
    {
        agent.destination = player.transform.position;
    }

    private void RangedFollow()
    {
        LookTarget();
        if(Vector3.Distance(transform.position, player.transform.position) > range)
            agent.destination = player.transform.position;
        else
        {
            agent.destination = transform.position;
            LookTarget();
            if(canAttackPlayer)
                RangedAttack();
        }
    }

    private void TikbalangAttack()
    {
        if(GameManager.Instance.enemiesArePaused)
            return;

        if(!canSpecialAction)
        {
            TikbalangFollow();
            specialActionCtr -= Time.deltaTime;

            if(specialActionCtr <= 0)
                canSpecialAction = true;
        }
        else
        {
            RandomAction();
            specialActionCtr = specialActionTimer;
            canSpecialAction = false;
        }
    }

    private void TornadoAttack()
    {
        if(GameManager.Instance.enemiesArePaused)
            return;

        Follow();

        if(!canSpecialAction)
        {
            specialActionCtr -= Time.deltaTime;

            if(specialActionCtr <= 0)
                canSpecialAction = true;
        }
        else
        {
            RandomAction();
            specialActionCtr = specialActionTimer;
            canSpecialAction = false;
        }
    }

    private void LookTarget()
	{
		Vector3 direction = (player.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, transform.position.y, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * (agent.angularSpeed/100));
        rigidbody.angularVelocity = Vector3.zero;
	}

    private void RandomAction()
    {
        EnemyActions.Do(actions[Random.Range(0, actions.Count)], this.gameObject, this);
    }

    void Die()
    {
        Drop();
        isAgressive = false;
        marker.SetActive(false);
        
        if(!independentBGM)
            if(added)
                GameManager.Instance.PopHostile(this);
        
        if(GameManager.Instance.GetHostileCount() == 0)
        {
            if(!independentBGM)
                AudioManager.Instance.PlayCalmBGM();
        }

        GetComponent<CapsuleCollider>().isTrigger = true;

        agent.speed = 0;
        isDead = true;

        this.enabled = false;
        animator.Play("Die");
        
        if(idleAudio)
            idleAudio.Stop();

        if(aggressionAudio)
            aggressionAudio.Stop();

        if(specialActionAudio)
            specialActionAudio.Stop();

        if(attackAudio)
            attackAudio.Stop();

        if(GetComponent<AudioSource>())
            GetComponent<AudioSource>().Stop();

        Destroy(gameObject, 2f);
    }

    void Drop()
    {
        Vector3 spawn = transform.localPosition;
        foreach(GameObject drop in drops)
        {
            GameObject prefab = Instantiate(drop);
            drop.transform.position = spawn;
        }

        if(Random.Range(1, 100) < chance)
        {
            foreach(GameObject drop in chanceDrops)
            {
                GameObject prefab = Instantiate(drop);
                drop.transform.position = spawn;
            }
        }
    }
    
    // comment out on build
    // private bool started = false;
    // void OnDrawGizmos()
	// {
	// 	if(!started)
	// 	{
	// 		originPosition = transform.position;
	// 		started = true;
	// 	}
			
	// 	Vector3 roamRadiusOrigin = new Vector3(originPosition.x, 0, originPosition.z);
	// 	Handles.color = Color.yellow;
	// 	Handles.DrawWireDisc(roamRadiusOrigin, transform.up, roamRadius);
	// }

    void OnDestroy()
    {
        
    }

}
