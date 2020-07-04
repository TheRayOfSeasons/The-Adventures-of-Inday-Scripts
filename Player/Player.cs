using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStats;
    [SerializeField] private float potionRestoreAmount = 35f;

    public float hitPoints;
    public float manaPoints;

    public float maxHitPoints;
    public float maxManaPoints;

    public int healthPotions;
    public int manaPotions;

    public bool invincible = false;

    [SerializeField] private float originalDamage;
    [SerializeField] private GameObject castPointRadius;
    [SerializeField] private Transform front;
    [SerializeField] private Transform frontDir;
    [SerializeField] private Transform backDir;
    [SerializeField] private Transform leftDir;
    [SerializeField] private Transform rightDir;
    [SerializeField] private float midAirMovement;
    [SerializeField] private float shootingTurnRate;

    [Header("VFX")]
    [SerializeField] private Renderer McFlap;
    [SerializeField] private Color InvincibilityColor;
    [SerializeField] private GameObject InvincibilityBarrier;
    [SerializeField] private Expand wind;
    [SerializeField] private GameObject radianceFX;
    public float damage;

    private Rigidbody rigidBody;

    // Double damage
    private float doubleDamageTimer;
    public bool isDoubleDamage = false;

    // Radiance
    public bool isRadianceOn = false;
    private float radianceTimer;

    // Tulak
    public bool canPush; 
    private float canPushTimer;

    private float invincibleTimer;

    // controls
    public KeyCode forward = KeyCode.W,
                   back = KeyCode.S,
                   left = KeyCode.A,
                   right = KeyCode.D,
                   jump = KeyCode.Space,
                   crouch = KeyCode.C,
                   run = KeyCode.LeftShift;

    void Start()
    {
        GameManager.Instance.Player = this.gameObject;
        rigidBody = GetComponent<Rigidbody>();
        healthPotions = playerStats.healthPotions;
        manaPotions = playerStats.manaPotions;
        ResetDamage();
    }

    void Update()
    {
        if(GameManager.Instance.IsPaused)
            return;
            
        if(ThirdPersonUserControl.Instance.Character.IsGrounded)
            return;

        if(Input.GetKey(KeyCode.W))
            HandleMidairMovement(GetDirection(frontDir), midAirMovement);
        else if(Input.GetKey(KeyCode.S))
            HandleMidairMovement(GetDirection(backDir), midAirMovement);

        if(Input.GetKey(KeyCode.A))
            HandleMidairMovement(GetDirection(leftDir), midAirMovement);
        else if(Input.GetKey(KeyCode.D))
            HandleMidairMovement(GetDirection(rightDir), midAirMovement);
    }

    private void HandleMidairMovement(Vector3 direction, float moveSpeed)
    {
        rigidBody.AddForce(direction * moveSpeed);
    }

    private Vector3 GetDirection(Transform direction)
    {
        return (direction.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.IsPaused)
            return;

        if(isDoubleDamage)
        {
            if(doubleDamageTimer > 0)
                doubleDamageTimer -= Time.fixedDeltaTime;
            else
            {
                isDoubleDamage = false;
                doubleDamageTimer = 0f;
                UIManager.Instance.CloseBuff(UIManager.Instance.doubleDamageIndex);
                ResetDamage();
            }

            UIManager.Instance.UpdateBuffTimer(Mathf.Round(doubleDamageTimer).ToString(), UIManager.Instance.doubleDamageIndex);
        }

        if(invincible)
        {
            if(invincibleTimer > 0)
                invincibleTimer -= Time.fixedDeltaTime;
            else
            {
                invincible = false;
                invincibleTimer = 0f;
                UIManager.Instance.CloseBuff(UIManager.Instance.invinicibilityIndex);
                SpellManager.InvincibilityActive = false;
                UIManager.Instance.ClearSpellStats();
                EnableInvincibilityBarrier(false);
            }

            UIManager.Instance.UpdateBuffTimer(Mathf.Round(invincibleTimer).ToString(), UIManager.Instance.invinicibilityIndex);
        }

        if(isRadianceOn)
        {
            if(radianceTimer > 0)
                radianceTimer -= Time.fixedDeltaTime;
            else
            {
                isRadianceOn = false;
                radianceTimer = 0f;
                UIManager.Instance.CloseBuff(UIManager.Instance.radianceIndex);
                SpellManager.RadianceActive = false;
                UIManager.Instance.ClearSpellStats();
                AudioManager.Instance.arawExit.Play();
                radianceFX.SetActive(false);
            }

            UIManager.Instance.UpdateBuffTimer(Mathf.Round(radianceTimer).ToString(), UIManager.Instance.radianceIndex);
        }

        if(canPush)
        {
            if(canPushTimer > 0)
            {
                canPushTimer -= Time.fixedDeltaTime;
            }
            else
            {
                canPush = false;
                canPushTimer = 0f;
                UIManager.Instance.CloseBuff(UIManager.Instance.hanginIndex);
                SpellManager.HanginActive = false;
                UIManager.Instance.ClearSpellStats();
                AudioManager.Instance.hanginExit.Play();
            }

            UIManager.Instance.UpdateBuffTimer(Mathf.Round(canPushTimer).ToString(), UIManager.Instance.hanginIndex);
        }
    }

    // fall damage in progress
    void OnCollisionEnter(Collision collision)
    {
        // if(rigidBody.velocity.y > 10)
        //     Damage(rigidBody.velocity.y);
    }

    public void MultiplyDamage(float multiplier, float time)
    {
        damage *= multiplier;
        isDoubleDamage = true;
        doubleDamageTimer = time;
    }

    private void ResetDamage()
    {
        damage = originalDamage;
    }

    public void AddHealthPotions()
    {
        playerStats.healthPotions++;
        healthPotions++;
    }

    public void AddManaPotions()
    {
        playerStats.manaPotions++;
        manaPotions++;
    }

    void UseHealthPotion()
    {
        playerStats.healthPotions--;
        healthPotions--;

        Heal(potionRestoreAmount);
    }

    void UseManaPotion()
    {
        playerStats.manaPotions--;
        manaPotions--;

        RestoreMana(potionRestoreAmount);
    }

    public void RestoreToFull()
    {
        hitPoints = maxHitPoints;
        manaPoints = maxManaPoints;
        UIManager.Instance.UpdatePlayerHealth();
        UIManager.Instance.UpdatePlayerMana();
    }

    public void Heal(float amount)
    {
        hitPoints += validatedValue(hitPoints, amount, maxHitPoints);
        UIManager.Instance.UpdatePlayerHealth();
    }

    public void RestoreMana(float amount)
    {
        manaPoints += validatedValue(manaPoints, amount, maxManaPoints);
        UIManager.Instance.UpdatePlayerMana();
    }

    public void Damage(float amount)
    {
        if(GameManager.Instance.Win)
            return;

        if(invincible)
            return;

        AudioManager.Instance.oof.Play();
        hitPoints -= amount;
        UIManager.Instance.UpdatePlayerHealth();
        UIManager.Instance.DamageBlur();

        if (hitPoints <= 0)
            Die();
    }

    public void MakeInvincible(float time)
    {
        invincible = true;
        invincibleTimer = time;
        EnableInvincibilityBarrier(true);
    }

    public void Radiate(float time, float dps)
    {
        castPointRadius.GetComponent<CastPointRadius>().SetPoints(time, dps);
        isRadianceOn = true;
        radianceTimer = time;

        radianceFX.SetActive(true);
    }

    public void PushObjects(float time, float force)
    {
        castPointRadius.GetComponent<CastPointRadius>().SetPoints(time, force);
        canPush = true;
        canPushTimer = time;

        wind.Initiate();
    }

    public void Knockback(Vector3 direction, float multiplier)
    {
        if(GameManager.Instance.Win)
            return;
            
        rigidBody.velocity += direction * multiplier;
        rigidBody.velocity += Vector3.up * multiplier;
    }

    public bool UseMana(float amount)
    {
        if (amount > manaPoints)
            return false;

        manaPoints -= amount;
        UIManager.Instance.UpdatePlayerMana();
        return true;
    }

    public void EnableInvincibilityBarrier(bool toggle)
    {
        Material[] materials = McFlap.materials;

        foreach(Material material in materials)
            material.color = toggle ? InvincibilityColor : Color.white;

        InvincibilityBarrier.SetActive(toggle);
    }

    public void LookForward()
    {
        Vector3 direction = (front.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * shootingTurnRate);
        rigidBody.angularVelocity = Vector3.zero;
    }

    public void Die()
    {
        GameManager.Instance.Lose();
        SlingShot sling = GetComponent<SlingShot>();

        sling.ResetSlingMode();
        sling.PlayerAnimator.SetBool("Aiming", false);
    }

    private float validatedValue(float toChange, float amount, float max)
    {
        if (toChange + amount > max)
            return max - toChange; 
        else
            return amount;
    }
}
