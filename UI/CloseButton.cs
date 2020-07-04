using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private enum Type
    {
        Archive,
        Crafting,
        Item
    };

    [SerializeField] private Type type;

    public void OnPointerDown(PointerEventData eventData) {}

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(type)
        {
            case Type.Archive:
                UIManager.Instance.ManualToggleArchivesMenu(false);
                break;
            case Type.Crafting:
                UIManager.Instance.CloseSpellCraftingMode();
                break;
            case Type.Item:
                UIManager.Instance.CloseItemUI();
                break;
        }
    }
}
