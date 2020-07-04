using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<Item> shopItems;

    public static void Buy(Item item)
    {
        if (InventoryManager.Instance.Full())
            return;

        if (GameManager.Instance.Gold < item.BuyPrice)
            return;

        GameManager.Instance.DecreaseGold(item.BuyPrice);
        InventoryManager.Instance.AddItem(item);
        AudioManager.Instance.gold.Play();

        if (GameManager.Instance.Gold < item.BuyPrice)
            UIManager.Instance.EnableUseOrBuyButton(false);
    }

    public static void Sell(Item item)
    {
        GameManager.Instance.AddGold(item.SellPrice);
        AudioManager.Instance.gold.Play();
        
        if(item.Stackable)
        {
            if(item.Stacks <= 1)
                InventoryManager.Instance.RemoveItem(item);
            else
                item.Stacks -= 1;
        }
        else
        {
            InventoryManager.Instance.RemoveItem(item);
        }

        UIManager.Instance.UpdateStacks(
            InventoryManager.Instance.inventory.items,
            InventoryManager.Instance.GetItemCount()
        );
    }

    public static void SellByStacks(Item item, int stacks)
    {
        if(!item.Stackable)
            return;

        if(item.Stacks >= stacks)
        {
            item.Stacks -= stacks;
            GameManager.Instance.AddGold(item.SellPrice * stacks);
            AudioManager.Instance.gold.Play();

            if(item.Stacks <= 1)
            {
                InventoryManager.Instance.RemoveItem(item);
                UIManager.Instance.EnableStashOrSellButton(false);
                return;
            }

            UIManager.Instance.UpdateStacks(
                InventoryManager.Instance.inventory.items,
                InventoryManager.Instance.GetItemCount()
            );
        }
    }
}
