using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource Calm;
    [SerializeField] private AudioSource Combat;
    [SerializeField] private AudioSource CombatResolve;
    [SerializeField] private AudioSource CombatStart;

    [Space]
    [Header("Inday")]
    public AudioSource oof;

    [Header("Spell SFX")]
    public AudioSource apoy;
    public AudioSource yelo;
    public AudioSource tibay;
    public AudioSource araw;
    public AudioSource galing;
    public AudioSource hangin;

    [Header("Spell Exit SFX")]
    public AudioSource hanginExit;
    public AudioSource arawExit;

    [Header("Slingshot")]
    public AudioSource slingStretch;
    public AudioSource slingRelease;

    [Header("Checkpoint")]
    public AudioSource checkpoint;

    [Header("Inventory")]
    public AudioSource inventory;
    public AudioSource potionUse;
    public AudioSource slingAmmoItemUse;

    [Header("Item Pickup")]
    public AudioSource gold;
    public AudioSource potionPickup;
    public AudioSource slingItemPickup; 

    [Header("General")]
    public AudioSource access;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        Calm.Play();
    }

    public void PlayCalmBGM()
    {
        if(Calm.isPlaying)
            return;

        Combat.Stop();
        CombatResolve.Play();
        Calm.Play();
    }

    public void PlayCombatBGM()
    {
        if(GameManager.Instance.Win)
            return;
            
        if(Combat.isPlaying)
            return;

        Calm.Stop();
        CombatStart.Play();
        Combat.Play();
    }
}
