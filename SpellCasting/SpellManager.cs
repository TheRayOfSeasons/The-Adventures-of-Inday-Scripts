using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private static SpellManager instance;
    public static SpellManager Instance
    {
        get { return instance; }
    }
    public Spell[] spells;

    [SerializeField] private Transform front;
    private static Transform staticFront;
    [SerializeField] private SpellSource spellSource;
    private static SpellSource staticSpellSource;
    [SerializeField] private GameObject fireProjectile;
    private static GameObject staticFireProjectile;
    [SerializeField] private GameObject iceProjectile;
    private static GameObject staticIceProjectile;
    [SerializeField] private static int charges = 0;

    private static bool justCasted = true;

    private static bool overridesSling = false;
    public bool SlingOverride
    {
        get { return overridesSling; } 
    }

    public static bool InvincibilityActive;
    public static bool RadianceActive;
    public static bool HanginActive;

    public delegate void SpellDelegate();
    public enum SpellName
    {
        apoy,
        yelo,
        tibay,
        araw,
        galing,
        hangin
    };

    public SpellDelegate[] SpellDelegates = 
    {
        apoy,
        yelo,
        tibay,
        araw,
        galing,
        hangin
    };

    public SpellDelegate currentSpellDelegate;
    public Spell currentSpell;
    private static Spell staticCurrentSpell;

    private Player player;
    private static Player staticPlayer;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        staticFront = front;
        staticFireProjectile = fireProjectile;
        staticIceProjectile = iceProjectile;
        staticSpellSource = spellSource;

        player = GameManager.Instance.Player.GetComponent<Player>();
        staticPlayer = player;

        bool[] unlockBooleans = GameManager.Instance.spells.GetAll();

        for(int i = 0; i < spells.Length; i++)
            spells[i].unlocked = unlockBooleans[i];

        UIManager.Instance.ToggleSpellCharges(charges);
    }

    public void Cast(Spell spell)
    {
        if(player.UseMana(spell.manaCost))
        {
            if(!spell.isChargesType)
            {
                overridesSling = false;
                ResetCharges();
                GetSpellDelegate(spell.effect)();
            }
            else
            {
                overridesSling = true;
                SetCharges(spell.charges);
            }

            UponCastEvents(spell);
            UIManager.Instance.UpdateCastedSpellBaybayinName(spell.BaybayinName);
            UIManager.Instance.SetCastedSpellImage(spell.sprite);
        }
        else
        {
            UIManager.Instance.ShowNotEnoughMana();
        }
    }

    public SpellDelegate GetSpellDelegate(SpellName spellName)
    {
        return SpellDelegates[(int)spellName];
    }

    public void SpellCheck()
    {
        foreach (Spell spell in spells)
        {
            if (GameManager.Instance.GetKeyboardTextValue() == spell.spellCode && spell.unlocked)
            {
                justCasted = true;
                currentSpell = spell;
                staticCurrentSpell = spell;
                currentSpellDelegate = GetSpellDelegate(spell.effect);
                GameManager.Instance.ManualToggleKeyboard(false);
                Cast(spell);

                return;
            }
        }

        UIManager.Instance.InvalidEntry();
    }

    public void UnlockSpell(Spell spell)
    {
        spell.unlocked = true;
    }

    public void ResetSpellsUnlocked()
    {
        foreach(Spell spell in spells)
            spell.unlocked = false;
    }

    public static void SetCharges(int amount)
    {
        charges = amount;
        UpdateChargesToUI(amount);
    }

    public static void UseCharges(int amount)
    {
        charges -= amount;
        UpdateChargesToUI(charges);

        if(charges <= 0)
        {
            overridesSling = false;
            UIManager.Instance.ClearSpellStats();
        }
    }

    public static void AddCharges(int amount)
    {
        charges += amount;
        UpdateChargesToUI(charges);
    }

    public static void ResetCharges()
    {
        charges = 0;
        UpdateChargesToUI(charges);
    }

    private static void UpdateChargesToUI(int amount)
    {
        UIManager.Instance.UpdateSpellCharges(amount);
    }

    public static void apoy()
    {
        JustCasted("Casted Apoy");

        if (charges > 0)
            Shoot(staticFireProjectile);
    }

    public static void yelo()
    {
        JustCasted("Casted Yelo");

        if (charges > 0)
            Shoot(staticIceProjectile);
    }

    public static void tibay()
    {
        JustCasted("Casted Tibay");

        staticPlayer.MakeInvincible(staticCurrentSpell.duration);

        if(InvincibilityActive)
            return;

        InvincibilityActive = true;
        UIManager.Instance.invinicibilityIndex = UIManager.Instance.AddBuff(staticCurrentSpell.sprite);
    }

    public static void araw()
    {
        JustCasted("Casted Araw");

        staticPlayer.Radiate(staticCurrentSpell.duration, staticCurrentSpell.points);

        if(RadianceActive)
            return;

        RadianceActive = true;
        UIManager.Instance.radianceIndex = UIManager.Instance.AddBuff(staticCurrentSpell.sprite);
    }

    public static void galing()
    {
        JustCasted("Casted Galing");

        staticPlayer.Heal(staticCurrentSpell.points);
    }

    public static void hangin()
    {
        JustCasted("Casted Hangin");

        staticPlayer.PushObjects(staticCurrentSpell.duration, staticCurrentSpell.points);

        if(HanginActive)
            return;

        HanginActive = true;
        UIManager.Instance.hanginIndex = UIManager.Instance.AddBuff(staticCurrentSpell.sprite);
    }

    private static void Shoot(GameObject projectile)
    {
        GameObject prefab = Instantiate(projectile);
        prefab.transform.position = staticSpellSource.transform.position;

        if (prefab.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
            Vector3 direction = (
                staticFront.position -
                staticSpellSource.transform.position
            ).normalized;
            rigidbody.velocity = direction * 30f;
        }

        Destroy(prefab, 2.5f);
        UseCharges(1);
    }

    private static void JustCasted(string log)
    {
        if (justCasted)
        {
            Debug.Log(log);
            justCasted = false;
        }
    }

    private void UponCastEvents(Spell spell)
    {
        switch(spell.Name)
        {
            case "Apoy":
                AudioManager.Instance.apoy.Play();
                break;
            case "Yelo":
                AudioManager.Instance.yelo.Play();
                break;
            case "Tibay":
                AudioManager.Instance.tibay.Play();
                break;
            case "Araw":
                AudioManager.Instance.araw.Play();
                break;
            case "Galing":
                AudioManager.Instance.galing.Play();
                break;
            case "Hangin":
                AudioManager.Instance.hangin.Play();
                break;
        }
    }
}
