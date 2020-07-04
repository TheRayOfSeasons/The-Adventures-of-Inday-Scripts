using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;    
    }

    public delegate void ItemAction();
    public enum ItemNames
    {
        HealthPotion,
        ManaPotion,
        DoubleDamage,
        Ammo,
        Gold,
        TestItem
    };

    public ItemAction[] itemActions =
    {
        HealthPotion,
        ManaPotion,
        DoubleDamage,
        Ammo,
        Gold,
        TestItem
    };

    private static Player player;
    private static Item currentItem;

    void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();    
    }

    public void Use(Item item)
    {
        currentItem = item;
        GetItemAction(item.Name)();
        InventoryManager.Instance.AfterItemUse(item);

        switch(item.Name)
        {
            case ItemNames.Ammo:
                AudioManager.Instance.slingAmmoItemUse.Play();
                break;
            default:
                AudioManager.Instance.potionUse.Play();
                break;
        }
    }

    public void InstantlyUse(Item item)
    {
        currentItem = item;
        GetItemAction(item.Name)();
    }

    public ItemAction GetItemAction(ItemNames itemName)
    {
        return itemActions[(int)itemName];
    }

    public static void HealthPotion()
    {
        player.Heal(25f);
    }

    public static void ManaPotion()
    {
        player.RestoreMana(25f);
    }

    public static void DoubleDamage()
    {
        player.MultiplyDamage(2f, currentItem.points);
        UIManager.Instance.doubleDamageIndex = UIManager.Instance.AddBuff(currentItem.sprite);
        UIManager.Instance.UpdateBuffTimer(currentItem.points.ToString(), UIManager.Instance.doubleDamageIndex);
    }

    public static void Ammo()
    {
        player.GetComponent<SlingShot>().AddAmmo(currentItem.points);
    }

    public static void Gold()
    {
        GameManager.Instance.AddGold(currentItem.points);
    }

    public static void TestItem()
    {
        Debug.Log("Test Item");
    }
}
