using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManager : MonoBehaviour
{
    
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public bool CanAttack;
    public GameObject Player;

    [Header("Checkpoint")]
    public Transform checkpoint;

    //Keyboard related fields
    [Header("Keyboard")]
    public GameObject baybayinKeyboard;
    public KeyboardIntegrator keyboardIntegrator;
    public UnlockedCharacters KeyboardCharacters;
    public UnlockedSpells spells;

    [Header("Baybayin Data")]
    public BaybayinDataList baybayinData;

    //Passcode related fields
    private string currentPasscode;
    public string CurrentPasscode
    {
        set { currentPasscode = value; }
    }
    private EventManager.EventDelegate eventDelegate;
    public EventManager.EventDelegate EventDelegate
    {
        set { eventDelegate = value; }
    }

    [SerializeField] private bool isPaused = false;
    public bool enemiesArePaused = false;
    public bool pauseUIInputs = false;
    public bool totalPause = false;
    public bool IsPaused
    {
        get { return isPaused; }
    }

    public int Gold;
    public bool MouseMovementActive = true;
    public bool PlayerInGameInputDisabled = false;
    public bool PlayerMovementActive = true;

    [Header("Tutorial Tests")]
    public bool Tutorial;
    public bool inventoryTutorialMode;
    public bool shopTutorialMode;
    public bool sellTutorialMode;

    public bool inventorySelectTutorialMode;

    public bool spellTutorialMode;
    public bool craftTutorialMode;
    public bool archiveTutorialMode;
    public bool baybayinArchiveTutorialMode;
    public bool passTutorialMode;

    public bool preArchiveTutorialMode;

    public List<Item> currentShopItems;

    [Header("Endgame Essentials")]
    public bool Win = false;
    public float EndTimer = 2f;
    [SerializeField] private GameObject FinalStage;
    private float EndTimeCounter;

    public enum KeyboardMode
    {
        EventMode,
        SpellMode,
        CraftMode
    }

    public KeyboardMode KeyMode;

    private List<Enemy> hostiles;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        hostiles = new List<Enemy>();
        ToggleTutorialMode(Tutorial);
        UIManager.Instance.UpdateGold(Gold); 
        checkpoint = Player.transform;
        KeyMode = KeyboardMode.SpellMode;
        EndTimeCounter = EndTimer;

        KeyboardCharacters.ResetConsonants();
        spells.ResetAll();
        InventoryManager.Instance.Reset();
        UIManager.Instance.ShowControls(true);
        // KeyboardCharacters.UnlockAllConsonants();
        // spells.UnlockAll();
    }

    void Update()
    {
        if(Win)
        {
            EndTimeCounter -= Time.deltaTime;

            if(EndTimeCounter <= 0)
            {
                LevelManager.Instance.InstantReturnToMenu();
            }

            return;
        }
            
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(UIManager.Instance.CanOpenNewUIPanel() || (DialogManager.Instance.isDialogRunning && !UIManager.Instance.IsPauseUIActive()))
            {
                Pause(true);
            }
            else
            {
                if(UIManager.Instance.IsPauseUIActive())
                {
                    Pause(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(UIManager.Instance.CanOpenNewUIPanel())
                ToggleSpellKeyboard(true);
            else
            {
                if(baybayinKeyboard.activeSelf && !DialogManager.Instance.isDialogRunning)
                    ToggleSpellKeyboard(false);
            }
        }

        if(baybayinKeyboard.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return))
                CompareInput();
        }
    }

    // With UI
    public void Pause(bool toggle)
    {
        totalPause = toggle;
        UIManager.Instance.TogglePauseUI(toggle);
        AudioManager.Instance.access.Play();

        if(!DialogManager.Instance.isDialogRunning)
        {
            PauseGame(toggle);
            UIManager.Instance.ToggleMainUI(!toggle);

            if(toggle)
                FreezeTime();
        }

    }

    // Only Pause Game Events
    public void PauseGame(bool toggle)
    {
        isPaused = toggle;
        PausePlayerInput(toggle);
        StopPlayerMovement(toggle);
        PauseEnemies(toggle);
        
        if (toggle)
        {
            FreeMouse();
        }
        else
        {
            LockMouse();
            NormalTime();
        }

        CanAttack = !toggle;
    }

    public void PauseEnemies(bool toggle)
    {
        enemiesArePaused = toggle;

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag(Tags.ENEMY))
            enemy.GetComponent<Enemy>().GetComponent<NavMeshAgent>().isStopped = toggle;
    }

    public void PauseUIInputs(bool toggle)
    {
        pauseUIInputs = toggle;
    }

    public void PausePlayerInput(bool toggle)
    {
        PlayerInGameInputDisabled = toggle;
        EnablePlayerMovement(!toggle);

        if (toggle)
            FreeMouse();
        else
            LockMouse();

        CanAttack = !toggle;
    }

    public void RestartToCheckpoint()
    {
        Player.GetComponent<Player>().RestoreToFull();
        CalmAllEnemies();
        Pause(false);
        UIManager.Instance.ShowLosePanel(false);
        Player.transform.position = checkpoint.position;
        ThirdPersonUserControl.Instance.StopMovement = false;
        PauseGame(false);
        PausePlayerInput(false);
        EnablePlayerMovement(true);
        NormalTime();
        MouseMovementActive = true;
    }

    // Mouse Related
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ConfineMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void FreeMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Gold Related
    public void AddGold(int amount)
    {
        Gold += amount;
        UIManager.Instance.UpdateGold(Gold);
    }

    public void DecreaseGold(int amount)
    {
        Gold -= amount;
        UIManager.Instance.UpdateGold(Gold);
    }

    // Keyboard Related Methods ----------------------------------------------------------
    public void ToggleSpellKeyboard (bool toggle)
    {
        KeyMode = KeyboardMode.SpellMode;
        UIManager.Instance.CloseInvalidEntry();
        baybayinKeyboard.SetActive(toggle);
        UIManager.Instance.Blur(toggle);
        UIManager.Instance.ResetGuideText();
        UIManager.Instance.DeactivateSpellUnlockAlert();
        PausePlayerInput(toggle);
        keyboardIntegrator.enable = toggle;

        if (toggle)
        {
            SlowMo();
            MouseMovementActive = false;

            if(AreAllKeyboardTutorialsDone())
                return;
            
            UIManager.Instance.KeyboardTutorial(KeyMode);
        }
        else
        {
            MouseMovementActive = true;
            NormalTime();
            ClearKeyboard();
            UIManager.Instance.ResetGuideText();
        }
    }

    public void ManualToggleKeyboard (bool toggle)
    {
        UIManager.Instance.CloseInvalidEntry();
        baybayinKeyboard.SetActive(toggle);
        UIManager.Instance.Blur(toggle);
        PausePlayerInput(toggle);
        UIManager.Instance.ToggleMainUI(!toggle);
        keyboardIntegrator.enable = toggle;

        if(KeyMode == KeyboardMode.SpellMode)
            UIManager.Instance.ResetGuideText();
        else if(KeyMode == KeyboardMode.EventMode || KeyMode == KeyboardMode.CraftMode)
        {
            StopPlayerMovement(toggle);
            PauseGame(toggle);
        }

        if (toggle)
        {
            if (KeyMode == KeyboardMode.SpellMode)
            {
                SlowMo();
                MouseMovementActive = false;
            }
            else
            {
                FreezeTime();
            }

            if(AreAllKeyboardTutorialsDone())
                return;
            
            UIManager.Instance.KeyboardTutorial(KeyMode);
        }
        else
        {
            MouseMovementActive = true;
            NormalTime();
            ClearKeyboard();
        }
    }

    public bool IsKeyboardEnabled ()
    {
        return baybayinKeyboard.activeSelf;
    }

    public void ClearKeyboard()
    {
        baybayinKeyboard.GetComponent<Keyboard>().Clear();
    }

    public string GetKeyboardTextValue ()
    {
        return baybayinKeyboard.GetComponent<Keyboard>().TextValue;
    }

    // Passcode Related Methods ----------------------------------------------------------
    public void CompareInput ()
    {
        switch(KeyMode)
        {
            case KeyboardMode.SpellMode:
                SpellManager.Instance.SpellCheck();
                break;
            case KeyboardMode.EventMode:
                PassCheck();
                break;
            case KeyboardMode.CraftMode:
                SpellCraftManager.Instance.ValidateSpellCraft();
                break;
        }
    }

    public void PassCheck()
    {
        Debug.Log(currentPasscode);
        if (eventDelegate != null && currentPasscode != null)
        {
            if (GetKeyboardTextValue() == currentPasscode)
            {
                AudioManager.Instance.access.Play();
                ManualToggleKeyboard(false);
                eventDelegate();
            }
            else
            {
                UIManager.Instance.InvalidEntry();
            }
        }
    }

    // Player Related
    public void EnablePlayerMovement(bool toggle)
    {
        PlayerMovementActive = toggle;
        ThirdPersonUserControl.Instance.EnablePlayerMovement = toggle;
    }

    public void StopPlayerMovement(bool toggle)
    {
        ThirdPersonUserControl.Instance.StopMovement = toggle;
    }

    public void NormalTime()
    {
        Time.timeScale = 1f;
    }

    public void SlowMo()
    {
        Time.timeScale = 0.2f;
    }

    public void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        PausePlayerInput(true);
        MouseMovementActive = false;
        UIManager.Instance.ShowLosePanel(true);
        EnablePlayerMovement(false);
        PauseGame(true);
        FreezeTime();
        CalmAllEnemies();
        ThirdPersonUserControl.Instance.StopMovement = true;
    }

    public void CalmAllEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag(Tags.ENEMY))
        {
            if(enemy.GetComponent<Enemy>().isAgressive)
                enemy.GetComponent<Enemy>().isAgressive = false;
        }
    }

    public void DeactivateAllEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag(Tags.ENEMY))
        {
            enemy.SetActive(false);
        }
    }

    // Enemy Related
    public int AddHostile(Enemy enemy)
    {
        hostiles.Add(enemy);
        // return hostiles.Count - 1;
        return 0;
    }

    public void PopHostile(Enemy enemy)
    {
        if(hostiles.Count <= 0)
            return;

        try
        {
            hostiles.Remove(enemy);
        }
        catch(System.NullReferenceException e)
        {
            Debug.Log("Object Not Found");
        }
            
        // hostiles.RemoveAt(index);
        // selected = hostiles[0];

        // if((selected = null) && (hostiles.Count <= 0))
        // {
        // }
    }

    public int GetHostileCount()
    {
        return hostiles.Count;
    }

    // public void SelectEnemy(Enemy enemy)
    // {
    //     if((selected = null))
    //     {
    //         selected = enemy;
    //     }
    // }

    // // Do this for Boss
    // public void PrioritySelectEnemy(Enemy enemy)
    // {
    //     selected = enemy;
    // }

    // Events
    public void DoEvent(EventManager.EventSelector eventSelector)
    {
        EventManager.GetDelegate(eventSelector)();
    }

    public void ToggleTutorialMode(bool toggle)
    {
        inventoryTutorialMode = toggle;
        shopTutorialMode = toggle;
        sellTutorialMode = toggle;
        inventorySelectTutorialMode = toggle;
        spellTutorialMode = toggle;
        craftTutorialMode = toggle;
        archiveTutorialMode = toggle;
        baybayinArchiveTutorialMode = toggle;
        passTutorialMode = toggle;
        preArchiveTutorialMode = toggle;
    }

    public void EndGame()
    {
        FinalStage.SetActive(false);
    }

    public bool AreAllItemTutorialsDone()
    {
        foreach(bool i in new bool[]{inventoryTutorialMode, shopTutorialMode, sellTutorialMode})
        {
            if(i)
                return false;
        }

        return true;
    }

    public bool AreAllKeyboardTutorialsDone()
    {
        foreach(bool i in new bool[]{passTutorialMode, craftTutorialMode, spellTutorialMode})
        {
            if(i)
                return false;
        }

        return true;
    }
}
