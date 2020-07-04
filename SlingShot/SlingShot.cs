using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    [SerializeField] private int ammo;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedStretchMultiplier;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform front;
    [SerializeField] private Transform attackPoint;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private CameraFollow camFollow;

    [Header("Animation")]
    [SerializeField] private SlingshotAnim slingAnim;

    private Player player;
    private float speed_o_meter;
    private bool slingInitiated;
    private bool inSlingMode;
    private Animator animator;
    public Animator PlayerAnimator
    {
        get { return animator; }
    }

    public void AddAmmo (int amount)
    {
        ammo += amount;
        UpdateUI();
    }

    void Start()
    {
        UpdateUI();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        inSlingMode = false;
        slingInitiated = false;
    }

    void Update()
    {
        if(GameManager.Instance.IsPaused)
            return;
            
        if(GameManager.Instance.CanAttack)
        {   
            if(!SpellManager.Instance.SlingOverride)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1))
                    InitiateSling();

                if(Input.GetKey(KeyCode.Mouse1) && slingInitiated)
                    SlingMode();

                if(Input.GetKeyUp(KeyCode.Mouse1) && inSlingMode)
                    ResetSlingMode();

                if(Input.GetKeyDown(KeyCode.Mouse0) && inSlingMode)
                    Shoot(speed_o_meter);
            } 
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
            animator.SetBool("Shoot", false);
    }

    void InitiateSling()
    {
        if(ammo <= 0)
            return;

        slingInitiated = true;
        inSlingMode = true;
        UIManager.Instance.ToggleSlingshotUI(true);
        UIManager.Instance.SetSlingShotConstraints(minSpeed, maxSpeed);
        GameManager.Instance.EnablePlayerMovement(false);
        GameManager.Instance.StopPlayerMovement(true);
        UIManager.Instance.ToggleCrosshair(true);
        animator.SetBool("Aiming", true);
        AudioManager.Instance.slingStretch.Play();
        slingAnim.PassToHand();
        
        camFollow.Zoom();
    }

    void SlingMode()
    {
        if(ammo <= 0)
            return;

        player.LookForward();
        if (speed_o_meter <= maxSpeed)
        {
            speed_o_meter += Time.deltaTime * speedStretchMultiplier;
            UIManager.Instance.UpdateSlingValue(speed_o_meter);
        }
    }

    public void ResetSlingMode()
    {
        speed_o_meter = minSpeed;
        inSlingMode = false;
        slingInitiated = false;
        GameManager.Instance.EnablePlayerMovement(true);
        GameManager.Instance.StopPlayerMovement(false);
        UIManager.Instance.ToggleCrosshair(false);
        UIManager.Instance.ToggleSlingshotUI(false);
        animator.SetBool("Aiming", false);
        animator.SetBool("Shoot", false);
        AudioManager.Instance.slingStretch.Stop();
        AudioManager.Instance.slingRelease.Play();

        camFollow.NormalDistance();
    }

    void Shoot(float velocity)
    {
        if(ammo > 0)
        {
            animator.SetBool("Shoot", true);
            GameObject prefab = Instantiate(projectile);
            prefab.transform.position = attackPoint.transform.position;

            if(prefab.GetComponent<Rigidbody>())
			{
                Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
                Vector3 direction = (
                    front.position -
                    attackPoint.transform.position
                ).normalized;   
                rigidbody.velocity = direction * velocity;

                if(prefab.GetComponent<PlayerProjectile>())
                {
                    PlayerProjectile projectile = prefab.GetComponent<PlayerProjectile>();
                    projectile.BonusDamage = velocity;
                }
			}

            ammo--;
            AudioManager.Instance.slingStretch.Play();
            AudioManager.Instance.slingRelease.Play();
            Destroy(prefab, 2.5f);
            ResetSling();
            UpdateUI();
        }
    }

    void ResetSling()
    {
        speed_o_meter = minSpeed;
    }

    void UpdateUI()
    {
        UIManager.Instance.UpdateSlingUI(ammo);
    }
}
