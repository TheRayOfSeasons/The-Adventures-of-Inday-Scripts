using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EventDelegate();
    public enum EventSelector
    {
        PrintOWO,
        PrintWOW,
        doNothing,
        UnlockB,
        UnlockK,
        UnlockD,
        UnlockG,
        UnlockH,
        UnlockL,
        UnlockM,
        UnlockN,
        UnlockNG,
        UnlockP,
        UnlockS,
        UnlockT,
        UnlockW,
        UnlockY,
        ShowVowels,
        CloseVowels,
        ShowModifiers,
        CloseModifiers,
        ShowSamplebaybayinModification,
        CloseSamplebaybayinModification,
        UnlockApoy,
        UnlockYelo,
        UnlockHangin,
        UnlockTibay,
        UnlockAraw,
        UnlockGaling,
        ShowShop,
        OpenSpellCrafting,
        UnpausePlayerInput,
        PausePlayerInput,
        GiveStartingGold,
        OpenCurrentDoor,
        CloseCurrentDoor,
        AcquireNotebook,
        EndGame,
        CalmAllEnemies
    };

    public static EventDelegate[] EventDelegates = 
    {
        PrintOWO,
        PrintWOW,
        DoNothing,
        UnlockB,
        UnlockK,
        UnlockD,
        UnlockG,
        UnlockH,
        UnlockL,
        UnlockM,
        UnlockN,
        UnlockNG,
        UnlockP,
        UnlockS,
        UnlockT,
        UnlockW,
        UnlockY,
        ShowVowels,
        CloseVowels,
        ShowModifiers,
        CloseModifiers,
        ShowSamplebaybayinModification,
        CloseSamplebaybayinModification,
        UnlockApoy,
        UnlockYelo,
        UnlockHangin,
        UnlockTibay,
        UnlockAraw,
        UnlockGaling,
        ShowShop,
        OpenSpellCrafting,
        UnpausePlayerInput,
        PausePlayerInput,
        GiveStartingGold,
        OpenCurrentDoor,
        CloseCurrentDoor,
        AcquireNotebook,
        EndGame,
        CalmAllEnemies
    };

    public static Door currentDoor;
    public static Animator currentShopAnimator;
    public static SpellCraftArea currentSpellCraftArea;

    public static EventDelegate GetDelegate(EventSelector eventSelector)
    {
        return EventDelegates[(int)eventSelector];
    }

    public static void DoNothing()
    {
        Debug.Log("Hello World");
    }

    public static void PrintOWO ()
    {
        Debug.Log("OWO");
    }

    public static void PrintWOW ()
    {
        Debug.Log("WOW");
    }

    public static void UnlockB()
    {
        GameManager.Instance.KeyboardCharacters.b = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.b);
    }

    public static void UnlockK()
    {
        GameManager.Instance.KeyboardCharacters.k = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.k);
    }

    public static void UnlockD()
    {
        GameManager.Instance.KeyboardCharacters.d = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.d);
    }

    public static void UnlockG()
    {
        GameManager.Instance.KeyboardCharacters.g = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.g);
    }

    public static void UnlockH()
    {
        GameManager.Instance.KeyboardCharacters.h = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.h);
    }

    public static void UnlockL()
    {
        GameManager.Instance.KeyboardCharacters.l = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.l);
    }

    public static void UnlockM()
    {
        GameManager.Instance.KeyboardCharacters.m = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.m);
    }

    public static void UnlockN()
    {
        GameManager.Instance.KeyboardCharacters.n = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.n);
    }

    public static void UnlockNG()
    {
        GameManager.Instance.KeyboardCharacters.ng = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.ng);
    }

    public static void UnlockP()
    {
        GameManager.Instance.KeyboardCharacters.p = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.p);
    }

    public static void UnlockS()
    {
        GameManager.Instance.KeyboardCharacters.s = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.s);
    }

    public static void UnlockT()
    {
        GameManager.Instance.KeyboardCharacters.t = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.t);
    }

    public static void UnlockW()
    {
        GameManager.Instance.KeyboardCharacters.w = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.w);
    }

    public static void UnlockY()
    {
        GameManager.Instance.KeyboardCharacters.y = true;
        UIManager.Instance.ShowUnlockedBaybayin(GameManager.Instance.baybayinData.y);
    }

    public static void ShowVowels()
    {
        UIManager.Instance.ShowVowels();
    }

    public static void CloseVowels()
    {
        UIManager.Instance.CloseVowels();
    }

    public static void ShowModifiers()
    {
        UIManager.Instance.ShowModifiers();
    }

    public static void CloseModifiers()
    {
        UIManager.Instance.CloseModifiers();
    }

    public static void ShowSamplebaybayinModification()
    {
        UIManager.Instance.ShowSamplebaybayinModification();
    }

    public static void CloseSamplebaybayinModification()
    {
        UIManager.Instance.CloseSamplebaybayinModification();
    }

    public static void UnlockApoy()
    {
        GameManager.Instance.spells.apoy = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void UnlockYelo()
    {
        GameManager.Instance.spells.yelo = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void UnlockHangin()
    {
        GameManager.Instance.spells.hangin = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void UnlockAraw()
    {
        GameManager.Instance.spells.araw = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void UnlockTibay()
    {
        GameManager.Instance.spells.tibay = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void UnlockGaling()
    {
        GameManager.Instance.spells.galing = true;
        UIManager.Instance.ShowNewSpellUnlockedAlert();
        UIManager.Instance.UpdateSpellsToArchive();
    }

    public static void ShowShop()
    {
        UIManager.Instance.UpdateShopToUI(GameManager.Instance.currentShopItems);
        UIManager.Instance.ManualToggleItemUI(true, UIManager.ItemUIType.Buy);
    }

    public static void OpenSpellCrafting()
    {
        UIManager.Instance.OpenSpellCraftingMode();
    }

    public static void UnpausePlayerInput()
    {
        GameManager.Instance.PausePlayerInput(false);
    }

    public static void PausePlayerInput()
    {
        GameManager.Instance.PausePlayerInput(true);
    }

    public static void GiveStartingGold()
    {
        GameManager.Instance.AddGold(500);
    }

    public static void OpenCurrentDoor()
    {
        if(currentDoor)
            currentDoor.Open();
    }

    public static void CloseCurrentDoor()
    {
        if(currentDoor)
            currentDoor.Close();
    }

    public static void AcquireNotebook()
    {
        GameManager.Instance.preArchiveTutorialMode = false;
        UIManager.Instance.ToggleNotebookAlert(true);
    }

    public static void EndGame()
    {
        OpenCurrentDoor();
        GameManager.Instance.EndGame();
        GameManager.Instance.PauseGame(true);
        GameManager.Instance.DeactivateAllEnemies();
        GameManager.Instance.Win = true;
        AudioManager.Instance.PlayCalmBGM();
        UIManager.Instance.Whiteout();
    }

    public static void CalmAllEnemies()
    {
        GameManager.Instance.CalmAllEnemies();
    }
}
