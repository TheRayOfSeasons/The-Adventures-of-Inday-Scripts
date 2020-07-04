using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    void Start()
    {
        foreach (Item item in items)
        {
            InventoryManager.Instance.AddItem(item);
        }
        UIManager.Instance.UpdateInventoryToUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (Item item in items)
            {
                InventoryManager.Instance.AddItem(item);
            }
        }
    }
}
