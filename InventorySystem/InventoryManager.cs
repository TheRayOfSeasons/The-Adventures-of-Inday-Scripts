using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public List<Item> validItems;
    public static InventoryManager Instance
    {
        get { return instance; }
    }

    public Inventory inventory;
    private int itemLimit;

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        itemLimit = UIManager.Instance.GetItemButtonsCount();
    }

    public int GetItemCount()
    {
        return inventory.items.Count;
    }

    public bool Full()
    {
        return !(inventory.items.Count < itemLimit - 1);
    }

    public void AddItem(Item item)
    {
        if (!Full())
        {
            if (item.Stackable)
            {
                foreach(Item inventoryItem in inventory.items)
                {
                    if (item.id == inventoryItem.id)
                    {
                        inventoryItem.Stacks += 1;
                        UIManager.Instance.UpdateStacks(inventory.items, GetItemCount());
                        return;
                    }
                }
            }
            inventory.items.Add(item);
        }
    }

    public void AfterItemUse(Item item)
    {
        if (item.Stackable)
        {
            if (item.Stacks > 1)
            {
                item.Stacks = item.Stacks - 1;
            }
            else
            {
                RemoveItem(item);
                UIManager.Instance.UpdateInventoryToUI();
            }
        }
        else
        {
            RemoveItem(item);
            UIManager.Instance.UpdateInventoryToUI();
        }
        UIManager.Instance.UpdateStacks(inventory.items, GetItemCount());
    }

    public void RemoveItem(Item item)
    {
        item.Stacks = 1;
        inventory.items.Remove(item);
        UIManager.Instance.UpdateInventoryToUI();
        AudioManager.Instance.inventory.Play();

        UIManager.Instance.ResetUseButton();
    }

    public void RemoveItems(Item item, int stacks)
    {
        item.Stacks -= stacks;

        if(item.Stacks <= 1)
        {
            RemoveItem(item);
            UIManager.Instance.UpdateStacks(inventory.items, GetItemCount());
            UIManager.Instance.UpdateInventoryToUI();
            UIManager.Instance.EnableUseOrBuyButton(false);
            UIManager.Instance.EnableStashOrSellButton(false);
            return;
        }

        AudioManager.Instance.inventory.Play();
        UIManager.Instance.UpdateStacks(inventory.items, GetItemCount());
        UIManager.Instance.ResetUseButton();
    }

    public void Reset()
    {
        int itemCount = inventory.items.Count;
        inventory.items.Clear();
        UIManager.Instance.HideStacks();
        UIManager.Instance.UpdateStacks(inventory.items, GetItemCount());

        foreach(Item item in validItems)
            item.Stacks = 1;
    }
}
