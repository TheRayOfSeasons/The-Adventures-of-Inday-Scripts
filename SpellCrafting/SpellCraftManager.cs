using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCraftManager : MonoBehaviour
{
    private static SpellCraftManager instance;
    public static SpellCraftManager Instance
    {
        get { return instance; }
    }

    public SpellShard currentShard;

    void Awake()
    {
        instance = this;    
    }

    public void Craft(SpellShard spellShard)
    {
        // interaction with Keyboard
        GameManager.Instance.KeyMode = GameManager.KeyboardMode.CraftMode;
        GameManager.Instance.ManualToggleKeyboard(true);
        UIManager.Instance.SetGuideText(spellShard.spell.Name);
        currentShard = spellShard;
    }

    public void ValidateSpellCraft()
    {
        if(GameManager.Instance.GetKeyboardTextValue() == currentShard.spell.spellCode)
        {
            currentShard.spell.unlocked = true;
            GameManager.Instance.DoEvent(currentShard.unlock);
            GameManager.Instance.ManualToggleKeyboard(false);
            EventManager.currentSpellCraftArea.unlocked = true;
            AudioManager.Instance.access.Play();

            if(EventManager.currentSpellCraftArea)
                EventManager.currentSpellCraftArea.InitiateUnlockDialog();
        }
        else
        {
            UIManager.Instance.InvalidEntry();
        }
    }
}
