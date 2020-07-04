using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private bool instantUse;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(instantUse)
                ItemManager.Instance.InstantlyUse(item);
            else
                InventoryManager.Instance.AddItem(item);

            ManageAudio(item);
            Destroy(gameObject);
        }
    }
    
    void Update () 
	{
		transform.localEulerAngles = new Vector3 (
			transform.localEulerAngles.x,
			transform.localEulerAngles.y + 5f,
			transform.localEulerAngles.z
		);
	}

    void ManageAudio(Item item)
    {
        switch(item.Name)
        {
            case ItemManager.ItemNames.Gold:
                AudioManager.Instance.gold.Play();
                break;
            case ItemManager.ItemNames.Ammo:
                AudioManager.Instance.slingItemPickup.Play();
                break;
            default:
                AudioManager.Instance.potionPickup.Play();
                break;
        }
    }
}
