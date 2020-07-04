using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    private GameObject Player;
    private bool isPaused = false;

    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    public bool UIInteractable = true;

    [Header("Main")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject spellCraftingMenu;

    [Header("Keyboard")]
    [SerializeField] private Text guide;

    [Header("Dialog")]
    [SerializeField] private Text interactionText;

    [Header("Lower Bar")]
    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider playerMP;
    [SerializeField] private Text playerHPText;
    [SerializeField] private Text playerMPText;

    [SerializeField] private Slider enemyHP;

    [Header("Interaction Text")]
    public bool HasInteractionText = false;

    [Header("Items")]
    [SerializeField] private GameObject ItemUI;
    [SerializeField] private List<Button> itemButtons;
    [SerializeField] private List<Image> itemButtonBackgrounds;
    [SerializeField] private Button UseOrBuy;
    [SerializeField] private Button StashOrSell;
    [SerializeField] private Text ItemDescription;
    [SerializeField] private Text ItemName;
    [SerializeField] private Text CostOrSellPrice;
    [SerializeField] private Text itemUIGold;
    [SerializeField] private Sprite ItemButtonEmptySprite;
    [SerializeField] private Button ItemUIClose;

    [Header("Stats")]
    [SerializeField] private Text ammo;
    [SerializeField] private Text spellCharges;
    [SerializeField] private Text mainUIGold;
    [SerializeField] private RectTransform BaybayinNameSourcePoint;
    [SerializeField] private Image castedSpellImage;

    [Header("Spell Crafting")]
    [SerializeField] private GameObject spellCraftingUI;
    [SerializeField] private Text spellShardName;
    [SerializeField] private Text spellShardDescription;
    [SerializeField] private Button craft;
    [SerializeField] private List<Button> spellShardButtons;

    [Header("Archives")]
    [SerializeField] private GameObject archivesUI;
    [SerializeField] private Button spellButton;
    [SerializeField] private Button baybayinButton;
    [SerializeField] private Button closeArchiveUI;
    [SerializeField] private GameObject archivesUnlocked;

    [Header("Baybayin Archives")]
    // Baybayin
    [SerializeField] private GameObject baybayinArchives;
    [SerializeField] private Image selectedBaybayin;
    [SerializeField] private Text baybayinRomanized;
    [SerializeField] private Text baybayinDescription;
    [SerializeField] private Text baybayinNotes;
    [SerializeField] private List<Button> baybayinButtons;

    public GameObject BaybayinArchive
    {
        get { return baybayinArchives; }
    }
    
    [Header("Spell Archives")]
    [SerializeField] private GameObject spellArchives;
    [SerializeField] private List<Button> spellButtons;
    [SerializeField] private RectTransform baybayinNamePoint;
    [SerializeField] private Text spellName;
    [SerializeField] private Text spellEnglishName;
    [SerializeField] private Text spellDescription;
    [SerializeField] private Text spellNotes;
    [SerializeField] private Text spellCost;
    [SerializeField] private Image spellImage;

    [Header("Blur")]
    [SerializeField] private Image BlurImage;
    [SerializeField] private Image DamageBlurImage;

    [SerializeField] private float BlurAlpha;
    [SerializeField] private float DamageBlurAlpha;

    [SerializeField] private float BlurTime;
    [SerializeField] private float DamageBlurTime;

    private bool inDamageBlur = false;
    private float damageBlurCtr = 0f;

    [Header("Baybayin Unlock")]
    [SerializeField] private GameObject BaybayinUnlockPanel;
    [SerializeField] private Image UnlockedBaybayinImage;
    [SerializeField] private Text UnlockedBaybayinName;
    [SerializeField] private Text UnlockedBaybayinDescription;
    [SerializeField] private Text UnlockedBaybayinNotes;

    [Header("Tutorial Mode Dialogs")]
    [SerializeField] private Dialog movementTutorial;
    [SerializeField] private Dialog preInventoryTutorial;
    [SerializeField] private Dialog preBaybayinUnlockTutorial;
    [SerializeField] private Dialog inventoryTutorial;
    [SerializeField] private Dialog shopTutorial;
    [SerializeField] private Dialog sellTutorial;

    [SerializeField] private Dialog spellCastingTutorial;
    [SerializeField] private Dialog craftingTutorial;
    [SerializeField] private Dialog preArchiveTutorial;
    [SerializeField] private Dialog archiveTutorial;
    [SerializeField] private Dialog baybayinArchiveTutorial;
    [SerializeField] private Dialog passCodeTutorial;

    [Header("Lose Panel")]
    [SerializeField] private GameObject LosePanel;

    [Header("Disable")]
    [SerializeField] private GameObject UIDisabler;

    [Header("Alert")]
    [SerializeField] private GameObject alert;
    [SerializeField] private Text alertText;

    private bool alertOn;
    private float alertTimer = 0f;

    [Header("New Spell Unlocked")]
    [SerializeField] private GameObject newSpellUnlockedAlert;

    [Header("Teaching Baybayin")]
    [SerializeField] private GameObject VowelPanel;
    [SerializeField] private GameObject ModifierPanel;
    [SerializeField] private GameObject SampleModificationPanel;
    
    [Header("Buffs")]
    [SerializeField] private GameObject buffPanel;
    [SerializeField] private Image buffImage;
    [SerializeField] private Text buffLabel;
    [SerializeField] private Text buffTime;

    [SerializeField] private List<GameObject> buffPanels;
    [SerializeField] private List<Image> buffImages;
    [SerializeField] private List<Text> buffLabels;
    [SerializeField] private List<Text> buffTimes;

    public int doubleDamageIndex = 0;
    public int invinicibilityIndex = 0;
    public int hanginIndex = 0;
    public int radianceIndex = 0;
    public int buffCount = 0;

    [Header("Not Enough Mana")]
    [SerializeField] private GameObject NotEnoughMana;
    [SerializeField] private float notEnoughManaTime;
    private float notEnoughManaTimer;

    [Header("Invalid Keyboard Entry")]
    [SerializeField] private GameObject invalidEntryPanel;

    [Header("Attacking")]
    [SerializeField] private GameObject crosshair;

    [Header("Item Deletion")]
    [SerializeField] private GameObject ConfirmationPanel;
    [SerializeField] private Text ConfirmationText;
    [SerializeField] private Button proceed;
    [SerializeField] private Slider toDeleteItemCount;
    [SerializeField] private Text deleteCounter;
    [SerializeField] private Button deleteCancel;
    private int previousDeleteCount;
    private delegate void ItemDestroyer(Item item, int stacks);

    [Header("Slingshot")]
    [SerializeField] private Slider slingshotSlider;

    [Header("Item UI Changing")]
    [SerializeField] private Sprite inventoryButton;
    [SerializeField] private Sprite shopButton;
    [SerializeField] private Sprite inventoryStuff;
    [SerializeField] private Sprite shopStuff;
    [SerializeField] private Sprite inventoryBG;
    [SerializeField] private Sprite shopBG;

    [Header("Controls")]
    [SerializeField] private GameObject controlsUI;

    [Header("EndGame Essentials")]
    [SerializeField] private Image whiteout;

    private ItemUIType itemUIChecker = ItemUIType.Inventory;
    public ItemUIType CurrentItemUI
    {
        get { return itemUIChecker; }
    }

    [SerializeField] private Button nocons;
    [SerializeField] private Button e_i;
    [SerializeField] private Button o_u;

    public enum ItemUIType
    {
        Inventory,
        Buy,
        Sell
    }

    private ItemUIType inventoryToggler;

    private string useButtonText = "Use",
                   stashButtonText = "Discard",
                   buyButtonText = "Buy",
                   sellButtonText = "Sell",
                   returnText = "Back To Shop";

    private float baybayinCharacterOffset = 50f;

    void Awake()
    {
        instance = this;    
    }

    void Start ()
    {
        notEnoughManaTimer = 0f;
        Player = GameManager.Instance.Player;
        inventoryToggler = ItemUIType.Inventory;
        previousDeleteCount = (int) toDeleteItemCount.value;

        ResetItemViewer(ItemUIType.Inventory);
        ResetBaybayinCharacterViewer();
        ResetSpellShardViewer();
        ResetSpellViewer();

        UpdateSpellsToArchive();
        UpdateSpellShardsToUI();
        UpdateInventoryToUI();
        UpdatePlayerHealth();
        UpdatePlayerMana();

        if(GameManager.Instance.Tutorial)
            TutorialDialog(movementTutorial, !TutorialManager.Instance.movementTestDone);
    }

    void Update()
    {
        if(GameManager.Instance.Win)
            return;
            
        if(toDeleteItemCount.gameObject.activeSelf)
        {
            if(toDeleteItemCount.value != previousDeleteCount)
            {
                deleteCounter.text = toDeleteItemCount.value.ToString();
                previousDeleteCount = (int) toDeleteItemCount.value;
            }
        }

        if(UIInteractable)
        {
            if(Input.GetKeyDown(KeyCode.I) && TutorialManager.Instance.movementTestDone)
            {
                if(CanOpenNewUIPanel())
                    ToggleInventory(true);
                else
                {
                    if(ItemUI.activeSelf)
                        ToggleInventory(false);
                }
            }
            if(Input.GetKeyDown(KeyCode.F1) && !GameManager.Instance.preArchiveTutorialMode)
            {
                if(CanOpenNewUIPanel())
                    ManualToggleArchivesMenu(true);
                else
                {
                    if(archivesUI.activeSelf)
                        ManualToggleArchivesMenu(false);
                }
            }

            if(Input.GetKeyDown(KeyCode.E) && BaybayinUnlockPanel.activeSelf && DialogManager.Instance.isDialogRunning)
                CloseUnlockedBaybayinPanel();

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(CanOpenNewUIPanel())
                    return;

                if(ItemUI.activeSelf)
                    CloseItemUI();
                if(archivesUI.activeSelf)
                    ManualToggleArchivesMenu(false);
                if(spellCraftingMenu.activeSelf)
                    CloseSpellCraftingMode();
                if(GameManager.Instance.baybayinKeyboard.activeSelf)
                    GameManager.Instance.ManualToggleKeyboard(false);
                if(BaybayinUnlockPanel.activeSelf)
                    CloseUnlockedBaybayinPanel();
                if(controlsUI.activeSelf)
                    ShowControls(false);
            }
        }

        if(!GameManager.Instance.Tutorial)
            return;

        if(TutorialManager.Instance.movementTestDone)
        {
            if(!TutorialManager.Instance.inventoryTestDone)
            {
                TutorialDialog(preInventoryTutorial, !TutorialManager.Instance.inventoryTestDone);
                TutorialManager.Instance.inventoryTestDone = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.Win)
        {
            return;
        }

        if(inDamageBlur)
        {
            if(damageBlurCtr > 0)
                damageBlurCtr -= Time.fixedDeltaTime;
            else
                ExitDamageBlur();
        }

        if(alertOn)
        {
            if(alertTimer > 0)
                alertTimer -= Time.fixedDeltaTime;
            else
                CloseAlert();
        }

        if(NotEnoughMana.activeSelf)
        {
            if(notEnoughManaTimer > 0)
                notEnoughManaTimer -= Time.fixedDeltaTime;
            else
                NotEnoughMana.SetActive(false);
        }
    }

    public void DisableUIInteraction(bool toggle)
    {
        UIDisabler.SetActive(toggle);
        UIInteractable = !toggle;
    }

    // Keyboard
    public void SetGuideText(string text)
    {
        guide.gameObject.SetActive(true);
        guide.text = text;
    }

    public void ResetGuideText()
    {
        guide.gameObject.SetActive(false);
        guide.text = "";
    }

    void LateUpdate()
    {
        nocons.interactable = true;
        e_i.interactable = true;
        o_u.interactable = true;    
    }

    // Item Related
    public int GetItemButtonsCount()
    {
        return itemButtons.Count;
    }

    public void CloseItemUI()
    {
        ManualToggleItemUI(false, ItemUIType.Inventory);

        if(!GameManager.Instance.Tutorial)
            return;

        if(TutorialManager.Instance.inventoryTestDone)
            TutorialDialog(preBaybayinUnlockTutorial, true);
    }

    public void ToggleInventory(bool toggle)
    {
        itemUIChecker = ItemUIType.Inventory;
        SwitchItemUIDesign(ItemUIType.Inventory);
        ItemUI.SetActive(toggle);
        // HandleItemTutorialGuide(type);
        Blur(ItemUI.activeSelf);
        UpdateInventoryToUI();
        AudioManager.Instance.inventory.Play();
        if (!ItemUI.activeSelf)
        {
            ResetItemViewer(ItemUIType.Inventory);
            inventoryToggler = ItemUIType.Inventory;
            EnableUseOrBuyButton(false);
            EnableStashOrSellButton(false);
            GameManager.Instance.NormalTime();

            if(GameManager.Instance.Tutorial && TutorialManager.Instance.inventoryTestDone)
                TutorialDialog(preBaybayinUnlockTutorial, true);
        }
        else
        {
            GameManager.Instance.FreezeTime();
        }
        SwitchItemButtonTexts(ItemUIType.Inventory);
        SetUIRamifications(ItemUI.activeSelf);
    }

    public void ManualToggleItemUI(bool toggle, ItemUIType type)
    {
        itemUIChecker = type;
        SwitchItemUIDesign(type);
        ItemUI.SetActive(toggle);
        // HandleItemTutorialGuide(type);
        ToggleMainUI(!toggle);
        AudioManager.Instance.inventory.Play();
        if(EventManager.currentShopAnimator)
        {
            EventManager.currentShopAnimator.ResetTrigger("Bow");
            EventManager.currentShopAnimator.SetTrigger("Bow");
        }
        Blur(toggle);
        if (!toggle)
        {
            inventoryToggler = ItemUIType.Inventory;
            ResetItemViewer(type);
            EnableUseOrBuyButton(false);
            EnableStashOrSellButton(false);
            GameManager.Instance.NormalTime();

            if(GameManager.Instance.Tutorial && TutorialManager.Instance.inventoryTestDone)
                TutorialDialog(preBaybayinUnlockTutorial, true);
        }
        else
        {
            GameManager.Instance.FreezeTime();
        }

        SwitchItemButtonTexts(type);
        SetUIRamifications(toggle);
    }

    private void SwitchItemUIDesign(ItemUIType type)
    {
        bool isInventory = type == ItemUIType.Inventory;
        ItemUI.GetComponent<Image>().sprite = isInventory ? inventoryBG : shopBG;
        ConfirmationPanel.GetComponent<Image>().sprite = isInventory ? inventoryBG : shopBG;
        proceed.image.sprite = isInventory ? inventoryButton : shopButton;
        deleteCancel.image.sprite = isInventory ? inventoryButton : shopButton;
        ItemUIClose.image.sprite = isInventory ? inventoryButton : shopButton;
        UseOrBuy.image.sprite = isInventory ? inventoryButton : shopButton;
        StashOrSell.image.sprite = isInventory ? inventoryButton : shopButton;

        foreach(Image i in itemButtonBackgrounds)
            i.sprite = isInventory ? inventoryStuff : shopStuff;

    }

    public void UpdateInventory()
    {
        List<Item> items = InventoryManager.Instance.inventory.items;
        int itemCount = InventoryManager.Instance.GetItemCount();

        ResetItemButtons();
        for(int i = 0; i < itemCount; i++)
        {
            itemButtons[i].onClick.AddListener(() => ViewItem(items[i], inventoryToggler));
            if(items[i].sprite)
            {
                itemButtons[i].image.sprite = items[i].sprite;
            }
        }
        for(int i = itemCount; i < GetItemButtonsCount(); i++)
        {
            itemButtons[i].interactable = false;
            itemButtons[i].image.sprite = ItemButtonEmptySprite;
        }
    }

    public void UpdateInventoryToUI()
    {
        UpdateItemsToUI(
            InventoryManager.Instance.inventory.items, 
            inventoryToggler, 
            InventoryManager.Instance.GetItemCount()
        );
    }

    public void UpdateShopToUI(List<Item> items)
    {
        itemUIChecker = ItemUIType.Buy;
        ResetItemViewer(ItemUIType.Inventory);
        inventoryToggler = ItemUIType.Inventory;
        UpdateItemsToUI(items, ItemUIType.Buy, items.Count);
        StashOrSell.onClick.AddListener(() => UpdateToSellUI());
        SwitchItemButtonTexts(ItemUIType.Buy);
        EnableUseOrBuyButton(false);
        EnableStashOrSellButton(true);
    }

    public void UpdateToSellUI()
    {
        itemUIChecker = ItemUIType.Sell;
        ResetItemViewer(ItemUIType.Inventory);
        inventoryToggler = ItemUIType.Sell;
        HandleItemTutorialGuide(ItemUIType.Sell);
        UseOrBuy.onClick.AddListener(() => UpdateShopToUI(GameManager.Instance.currentShopItems));
        SwitchItemButtonTexts(ItemUIType.Sell);
        UpdateItemsToUI(
            InventoryManager.Instance.inventory.items, 
            ItemUIType.Sell, 
            InventoryManager.Instance.GetItemCount()
        );
        EnableUseOrBuyButton(true);
        EnableStashOrSellButton(false);
    }

    public void EnableUseOrBuyButton(bool toggle)
    {
        UseOrBuy.interactable = toggle;
    }

    public void EnableStashOrSellButton(bool toggle)
    {
        StashOrSell.interactable = toggle;
    }

    public void ResetUseButton()
    {
        if(CurrentItemUI == ItemUIType.Inventory || CurrentItemUI == ItemUIType.Buy)
            UseOrBuy.interactable = false;
    }

    private void ViewItem(Item item, ItemUIType type)
    {
        ItemDescription.text = item.description;
        ResetItemViewer(type);

        ItemName.text = item.Name.ToString();
        switch(type)
        {
            case ItemUIType.Inventory:
                if(!UseOrBuy.interactable)
                    EnableUseOrBuyButton(true);
                if(!StashOrSell.interactable)
                    EnableStashOrSellButton(true);

                if(item.Useable)
                    UseOrBuy.onClick.AddListener(() => ItemManager.Instance.Use(item));

                StashOrSell.onClick.AddListener(() => 
                    ShowConfirmationPanel(
                        InventoryManager.Instance.RemoveItems, 
                        "Are you sure you want to discard this item?",
                        item
                    )
                );
                break;
            case ItemUIType.Buy:
                if(GameManager.Instance.Gold < item.BuyPrice)
                {
                    UseOrBuy.interactable = false;
                    return;
                }

                if(!UseOrBuy.interactable)
                    EnableUseOrBuyButton(true);

                CostOrSellPrice.text = "Cost: " + item.BuyPrice.ToString();
                UseOrBuy.onClick.AddListener(() => Shop.Buy(item));
                break;
            case ItemUIType.Sell:
                if(!StashOrSell.interactable)
                    StashOrSell.interactable = true;
                
                CostOrSellPrice.text = "Sell Price: " + item.SellPrice.ToString();
                StashOrSell.onClick.AddListener(() => 
                    ShowConfirmationPanel( 
                        Shop.SellByStacks, 
                        "Are you sure you want to sell this item?",
                        item
                    )
                );
                break;
        }
    }

    private void UpdateItemsToUI(List<Item> items, ItemUIType type, int itemCount)
    {
        ResetItemButtons();
        ItemDescription.text = "Select Item";
        
        for(int i = itemCount; i < itemButtons.Count; i++)
        {
            itemButtons[i].interactable = false;
            itemButtons[i].image.sprite = ItemButtonEmptySprite;
        }
        for(int i = 0; i < itemCount; i++)
        {
            Item item = items[i];
            Button button = itemButtons[i];
            Text stack = button.transform.GetChild(0).GetComponent<Text>();
            button.onClick.AddListener(() => ViewItem(item, type));
            button.interactable = true;

            if(item.sprite)
                button.image.sprite = item.sprite;
        }

        if(type == ItemUIType.Inventory || type == ItemUIType.Sell)
        {
            ShowStacks(items, itemCount);
            UpdateStacks(items, itemCount);
        }
        else
        {
            HideStacks();
        }
    }

    public void UpdateStacks(List<Item> items, int itemCount)
    {
        for(int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        for(int i = 0; i < itemCount; i++)
        {
            Item item = items[i];
            Button button = itemButtons[i];
            Text stack = button.transform.GetChild(1).GetComponent<Text>();

            if(itemUIChecker != ItemUIType.Buy)
                stack.gameObject.SetActive(true);

            if(item.Stackable)
                stack.text = item.Stacks.ToString();
        }
    }

    public void ShowStacks(List<Item> items, int itemCount)
    {
        for(int i = 0; i < itemCount; i++)
        {
            if(items[i].Stackable)
                itemButtons[i].transform.GetChild(1).GetComponent<Text>().gameObject.SetActive(true);
        }
    }

    public void HideStacks()
    {
        for(int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].transform.GetChild(1).GetComponent<Text>().gameObject.SetActive(false);
        }
    }

    private void SwitchItemButtonTexts(ItemUIType type)
    {
        switch(type)
        {
            case ItemUIType.Inventory:
                UseOrBuy.GetComponentInChildren<Text>().text = useButtonText;
                StashOrSell.GetComponentInChildren<Text>().text = stashButtonText;
                break;
            case ItemUIType.Buy:
                UseOrBuy.GetComponentInChildren<Text>().text = buyButtonText;
                StashOrSell.GetComponentInChildren<Text>().text = sellButtonText;
                break;
            case ItemUIType.Sell:
                UseOrBuy.GetComponentInChildren<Text>().text = returnText;
                StashOrSell.GetComponentInChildren<Text>().text = sellButtonText;
                break;
        }
    }

    private void ResetItemButtons()
    {
        foreach(Button button in itemButtons)
        {
            button.onClick.RemoveAllListeners();
            button.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetItemViewer(ItemUIType type)
    {
        ItemName.text = "";
        CostOrSellPrice.text = "";
        switch(type)
        {
            case ItemUIType.Inventory:
                UseOrBuy.onClick.RemoveAllListeners();
                StashOrSell.onClick.RemoveAllListeners();
                break;
            case ItemUIType.Buy:
                UseOrBuy.onClick.RemoveAllListeners();
                break;
            case ItemUIType.Sell:
                StashOrSell.onClick.RemoveAllListeners();
                break;
        }
    }

    private void ShowConfirmationPanel(ItemDestroyer buttonAction, string deleteText, Item item)
    {
        ConfirmationPanel.SetActive(true);
        toDeleteItemCount.gameObject.SetActive(item.Stackable);

        if(toDeleteItemCount.gameObject.activeSelf)
        {
            toDeleteItemCount.maxValue = item.Stacks;
            toDeleteItemCount.value = 1;
        }

        ConfirmationText.text = deleteText;
        proceed.onClick.RemoveAllListeners();
        proceed.onClick.AddListener(() => {
            buttonAction(item, (int) toDeleteItemCount.value);
            ConfirmationPanel.SetActive(false);
        });
    }

    // Dialog Related
    public void SetInteractionText(string description)
    {
        interactionText.text = description;
    }

    public void EnableInteractionText()
    {
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    // Main GUI
    public void ToggleMainUI(bool toggle)
    {
        mainUI.SetActive(toggle);
    }

    public void TogglePauseUI(bool toggle)
    {
        pauseUI.SetActive(toggle);
    }

    public void UpdatePlayerHealth()
    {
        playerHP.maxValue = Player.GetComponent<Player>().maxHitPoints;
        playerHP.value = Player.GetComponent<Player>().hitPoints;

        playerHPText.text = playerHP.value + " / " + playerHP.maxValue;
    }

    public void UpdatePlayerMana()
    {
        playerMP.maxValue = Player.GetComponent<Player>().maxManaPoints;
        playerMP.value = Player.GetComponent<Player>().manaPoints;

        playerMPText.text = playerMP.value + " / " + playerHP.maxValue;
    }

    public void ToggleEnemyStatus(bool toggle)
    {
        enemyHP.gameObject.SetActive(toggle);
    }

    public void UpdateEnemyStatus(Enemy enemy)
    {
        enemyHP.maxValue = enemy.maxHitPoints;
        enemyHP.value = enemy.hitPoints;
    }

    // Stats
    public void UpdateGold(int amount)
    {
        string goldText = "Gold: " + amount;
        mainUIGold.text = goldText;
        itemUIGold.text = goldText;
    }

    public void UpdateSlingUI(int ammoAmount)
    {
        ammo.text = "Sling: " + ammoAmount.ToString(); 
    }

    public void UpdateCastedSpellBaybayinName(List<GameObject> baybayinName)
    {
        float offset = baybayinCharacterOffset;
        float offsetUpdater = 0f;
        ClearBaybayinSpellname();

        for(int i = baybayinName.Count - 1; i >= 0; i--)
        {
            GameObject character = Instantiate(baybayinName[i]);
            character.transform.SetParent(BaybayinNameSourcePoint.transform);
            character.transform.position = new Vector3
            (
                BaybayinNameSourcePoint.transform.position.x - offsetUpdater,
                BaybayinNameSourcePoint.transform.position.y,
                BaybayinNameSourcePoint.transform.position.z
            );
            offsetUpdater += offset;
        }
    }

    public void ClearBaybayinSpellname()
    {
        foreach (Transform child in BaybayinNameSourcePoint.transform)
			Destroy(child.gameObject);
    }

    public void SetCastedSpellImage(Sprite sprite)
    {
        castedSpellImage.sprite = sprite;
        castedSpellImage.gameObject.SetActive(true);
    }

    public void ClearSpellStats()
    {
        ClearBaybayinSpellname();
        castedSpellImage.gameObject.SetActive(false);
    }

    public void UpdateSpellCharges(int charges)
    {
        spellCharges.text = charges.ToString();
        ToggleSpellCharges(charges);
    }

    public void ToggleSpellCharges(int charges)
    {
        if(charges <= 0)
            spellCharges.gameObject.SetActive(false);
        else
            spellCharges.gameObject.SetActive(true);
    }

    // Spell Crafting
    public void OpenSpellCraftingMode()
    {
        UpdateSpellShardsToUI();
        ToggleMainUI(false);
        spellCraftingMenu.SetActive(true);
        SetUIRamifications(true);

        if(!GameManager.Instance.craftTutorialMode)
            return;

        TutorialDialog(craftingTutorial, GameManager.Instance.craftTutorialMode);
        GameManager.Instance.craftTutorialMode = false;
    }

    public void CloseSpellCraftingMode()
    {
        ResetSpellShardViewer();
        ToggleMainUI(true);
        spellCraftingMenu.SetActive(false);
        SetUIRamifications(false);
    }

    public void UpdateSpellShardsToUI()
    {
        List<Item> items = InventoryManager.Instance.inventory.items;
        List<Item> shards = new List<Item>();

        foreach(Item item in items)
        {
            if(item.isSpellShard)
                shards.Add(item);
        }

        for(int i = 0; i < shards.Count; i++)
        {
            Item shard = shards[i];
            Button button = spellShardButtons[i];

            button.interactable = true;
            button.onClick.AddListener(() => ViewSpellShard(shard));

            if(shard.sprite)
                button.image.sprite = shard.sprite;
        }

        for(int i = shards.Count; i < spellShardButtons.Count; i++)
        {
            spellShardButtons[i].interactable = false;
        }
    }

    private void ViewSpellShard(Item shard)
    {
        spellShardName.text = shard.Name.ToString();
        spellShardDescription.text = shard.description;
        
        craft.interactable = true;
        craft.onClick.AddListener(() => SpellCraftManager.Instance.Craft(shard.spellShard));
    }

    private void ResetSpellShardViewer()
    {
        spellShardName.text = "";
        spellShardDescription.text = "";

        craft.interactable = false;
    }

    private void SetUIRamifications(bool toggle)
    {
        GameManager.Instance.PauseGame(toggle);
        GameManager.Instance.PausePlayerInput(toggle);
    }

    // Blurs
    public void Blur(bool toggle)
    {
        if(toggle)
            BlurImage.CrossFadeAlpha(BlurAlpha, BlurTime, true);
        else
            BlurImage.CrossFadeAlpha(1f, BlurTime, true);
    }

    public void Whiteout()
    {
        ToggleMainUI(false);
        whiteout.gameObject.SetActive(true);
        whiteout.CrossFadeAlpha(255, GameManager.Instance.EndTimer - 0.5f, true);
    }

    public void DamageBlur()
    {
        DamageBlurImage.CrossFadeAlpha(DamageBlurAlpha, DamageBlurTime, false);
        inDamageBlur = true;
        damageBlurCtr = DamageBlurTime;
    }

    private void ExitDamageBlur()
    {
        DamageBlurImage.CrossFadeAlpha(1f, DamageBlurTime, false);
        inDamageBlur = false;
        damageBlurCtr = 0;
    }

    // Archives
    public void ToggleArchivesMenu()
    {
        archivesUI.SetActive(!archivesUI.activeSelf);

        ToggleMainUI(!archivesUI.activeSelf);
        SetUIRamifications(archivesUI.activeSelf);

        if(!GameManager.Instance.archiveTutorialMode)
            return;
        
        TutorialDialog(archiveTutorial, GameManager.Instance.archiveTutorialMode);
        GameManager.Instance.archiveTutorialMode = false;
    }

    public void ManualToggleArchivesMenu(bool toggle)
    {
        archivesUI.SetActive(toggle);
        ToggleMainUI(!toggle);

        if(archivesUnlocked.activeSelf)
            ToggleNotebookAlert(false);

        TutorialDialog(archiveTutorial, GameManager.Instance.archiveTutorialMode);
        AudioManager.Instance.access.Play();
        GameManager.Instance.archiveTutorialMode = false;
        ToggleMainUI(!toggle);
        SetUIRamifications(toggle);
    }

    public void ToggleArchiveTabs(bool toggle)
    {
        switch(toggle)
        {
            case true:
                baybayinArchives.SetActive(false);
                spellArchives.SetActive(true);
                break;
            case false:
                baybayinArchives.SetActive(true);
                spellArchives.SetActive(false);

                if(!GameManager.Instance.baybayinArchiveTutorialMode)
                    return;

                TutorialDialog(baybayinArchiveTutorial, GameManager.Instance.baybayinArchiveTutorialMode);
                GameManager.Instance.baybayinArchiveTutorialMode = false;
                break;
        }
    }

    // Archives
    public void ViewBaybayinCharacter(Image image, string romanized, string description, string notes)
    {
        selectedBaybayin.sprite = image.sprite;
        baybayinRomanized.text = romanized;
        baybayinDescription.text = description;
        baybayinNotes.text = notes;
    }

    public void ResetBaybayinCharacterViewer()
    {
        baybayinRomanized.text = "";
        baybayinDescription.text = "";
        baybayinNotes.text = "";
    }

    public void UpdateSpellsToArchive()
    {
        Spell[] spells = SpellManager.Instance.spells;

        for(int i = 0; i < spells.Length; i++)
        {
            if(spells[i].unlocked)
            {
                Spell spell = spells[i];
                Button button = spellButtons[i];
                button.interactable = true;
                button.image.sprite = spell.sprite;
                button.onClick.AddListener(() => ViewSpell(spell));
            }
        }
    }

    public void ViewSpell(Spell spell)
    {
        ResetSpellViewer();

        float offsetUpdater = 0f;

        for(int i = 1; i < spell.BaybayinName.Count; i++)
        {
            offsetUpdater -= baybayinCharacterOffset / 2;
        }

        foreach(GameObject baybayin in spell.BaybayinName)
        {
            GameObject character = Instantiate(baybayin);
            character.transform.SetParent(baybayinNamePoint);
            character.transform.position = new Vector3
            (
                baybayinNamePoint.transform.position.x + offsetUpdater,
                baybayinNamePoint.transform.position.y,
                baybayinNamePoint.transform.position.z
            );

            offsetUpdater += baybayinCharacterOffset;
        }

        spellName.text = spell.Name;
        spellEnglishName.text = "(" + spell.EnglishName + ")";
        spellDescription.text = spell.description;
        spellNotes.text = spell.notes;
        spellCost.text = "Mana Cost: " + spell.manaCost;
        spellImage.sprite = spell.sprite; 
    }

    public void ResetSpellViewer()
    {
        foreach(RectTransform baybayin in baybayinNamePoint)
            Destroy(baybayin.gameObject);

        spellName.text = "";
        spellEnglishName.text = "";
        spellDescription.text = "";
        spellNotes.text = "";
        spellCost.text = "";
    }

    // Baybayin Unlock
    public void ShowUnlockedBaybayin(BaybayinData data)
    {
        if(DialogManager.Instance.isDialogRunning)
            DialogManager.Instance.canUnload = false;

        AudioManager.Instance.access.Play();
        ToggleMainUI(false);

        BaybayinUnlockPanel.SetActive(true);
        GameManager.Instance.PausePlayerInput(true);
        UnlockedBaybayinImage.sprite = data.image.sprite;
        UnlockedBaybayinName.text = data.romanized;
        UnlockedBaybayinDescription.text = data.description;
        UnlockedBaybayinNotes.text = data.notes;
    }

    public void CloseUnlockedBaybayinPanel()
    {
        if(!DialogManager.Instance.isDialogRunning)
        {
            GameManager.Instance.PauseGame(false);
            GameManager.Instance.PausePlayerInput(false);
            ToggleMainUI(true);
        }

        DialogManager.Instance.canUnload = true;

        BaybayinUnlockPanel.SetActive(false);
        UnlockedBaybayinName.text = "";
        UnlockedBaybayinDescription.text = "";
        UnlockedBaybayinNotes.text = "";

        if(!GameManager.Instance.Tutorial)
            return;

        if(GameManager.Instance.preArchiveTutorialMode)
        {
            TutorialDialog(preArchiveTutorial, GameManager.Instance.preArchiveTutorialMode);
            GameManager.Instance.preArchiveTutorialMode = false;
        }
    }

    // Tutorial
    private void HandleItemTutorialGuide(ItemUIType type)
    {
        if(GameManager.Instance.AreAllItemTutorialsDone())
            return;

        switch(type)
        {
            case ItemUIType.Inventory:
                if(!GameManager.Instance.inventoryTutorialMode)
                    return;

                TutorialDialog(inventoryTutorial, GameManager.Instance.inventoryTutorialMode);
                GameManager.Instance.inventoryTutorialMode = false;
                break;
            case ItemUIType.Buy:
                if(!GameManager.Instance.shopTutorialMode)
                    return;

                TutorialDialog(shopTutorial, GameManager.Instance.shopTutorialMode);
                GameManager.Instance.shopTutorialMode = false;
                break;
            case ItemUIType.Sell:
                if(!GameManager.Instance.sellTutorialMode)
                    return;

                TutorialDialog(sellTutorial, GameManager.Instance.sellTutorialMode);
                GameManager.Instance.sellTutorialMode = false;
                break;
        }
    }

    public void KeyboardTutorial(GameManager.KeyboardMode mode)
    {
        switch(mode)
        {
            case GameManager.KeyboardMode.CraftMode:
                if(!GameManager.Instance.craftTutorialMode)
                    return;

                TutorialDialog(craftingTutorial, GameManager.Instance.craftTutorialMode);
                GameManager.Instance.craftTutorialMode = false;
                break;
            case GameManager.KeyboardMode.EventMode:
                if(!GameManager.Instance.passTutorialMode)
                    return;
                    
                TutorialDialog(passCodeTutorial, GameManager.Instance.passTutorialMode);
                GameManager.Instance.passTutorialMode = false;
                break;
            case GameManager.KeyboardMode.SpellMode:
                if(!GameManager.Instance.spellTutorialMode)
                    return;
                    
                TutorialDialog(spellCastingTutorial, GameManager.Instance.spellTutorialMode);
                GameManager.Instance.spellTutorialMode = false;
                break;
        }
    }

    private void TutorialDialog(Dialog dialog, bool mode)
    {
        if(!mode)
            return;

        DialogManager.Instance.InitiateDialog(dialog, DialogSequence.Default());
        DialogManager.Instance.UnloadDialog();
    }

    public bool CanOpenNewUIPanel()
    {
        foreach(GameObject panel in new GameObject[]
            {
                pauseUI,
                spellCraftingMenu,
                GameManager.Instance.baybayinKeyboard,
                DialogManager.Instance.DialogPanel,
                ItemUI,
                spellCraftingUI,
                archivesUI,
                BaybayinUnlockPanel,
                controlsUI
            }
        )
        {
            if(panel.activeSelf)
                return false;
        }
        return true;
    }

    public bool IsPauseUIActive()
    {
        return pauseUI.activeSelf;
    }

    public void Alert(string text, float timer)
    {
        alert.SetActive(true);
        alertText.text = text;
        alertOn = true;
        alertTimer = timer;
    }

    private void CloseAlert()
    {
        alert.SetActive(false);
        alertText.text = "";
        alertOn = false;
        alertTimer = 0f;
    }

    public void ShowNewSpellUnlockedAlert()
    {
        newSpellUnlockedAlert.SetActive(true);
    }

    public void DeactivateSpellUnlockAlert()
    {
        if(newSpellUnlockedAlert.activeSelf)
            newSpellUnlockedAlert.SetActive(false);
    }

    public void ShowNotEnoughMana()
    {
        NotEnoughMana.SetActive(true);
        notEnoughManaTimer = notEnoughManaTime;
    }

    public void ShowVowels()
    {
        VowelPanel.SetActive(true);
    }

    public void CloseVowels()
    {
        VowelPanel.SetActive(false);
    }

    public void ShowModifiers()
    {
        ModifierPanel.SetActive(true);
    }

    public void CloseModifiers()
    {
        ModifierPanel.SetActive(false);
    }

    public void ShowSamplebaybayinModification()
    {
        SampleModificationPanel.SetActive(true);
    }

    public void CloseSamplebaybayinModification()
    {
        SampleModificationPanel.SetActive(false);
    }

    public void ShowLosePanel(bool toggle)
    {
        LosePanel.SetActive(toggle);
        gameUI.SetActive(!toggle);
    }

    // Buffs
    public void SetBuff(Sprite sprite, string label)
    {
        buffPanel.SetActive(true);
        buffImage.sprite = sprite;
        buffLabel.text = label;
    }

    public int AddBuff(Sprite sprite)
    {
        int i = ++buffCount;
        buffPanels[i].SetActive(true);
        buffImages[i].sprite = sprite;

        return i;
    }

    public void UpdateBuffTimer(string time)
    {
        buffTime.text = time;
    }

    public void UpdateBuffTimer(string time, int index)
    {
        buffTimes[index].text = time;
    }

    public void CloseBuff()
    {
        buffPanel.SetActive(false);
        buffLabel.text = "";
        buffTime.text = "";
    }

    public void CloseBuff(int index)
    {
        buffPanels[index].SetActive(false);
        buffTimes[index].text = "";

        buffCount--;
    }

    public void InvalidEntry()
    {
        invalidEntryPanel.SetActive(true);
    }

    public void CloseInvalidEntry()
    {
        invalidEntryPanel.SetActive(false);
    }

    public void ToggleCrosshair(bool toggle)
    {
        if(!Options.Instance.Data.EnableCrosshair)
        {
            crosshair.SetActive(false);
            return;
        }
        crosshair.SetActive(toggle);
    }

    public void ToggleSlingshotUI(bool toggle)
    {
        slingshotSlider.gameObject.SetActive(toggle);
    }

    public void SetSlingShotConstraints( float min, float max)
    {
        slingshotSlider.minValue = min;
        slingshotSlider.maxValue = max;
    }

    public void UpdateSlingValue(float value)
    {
        slingshotSlider.value = value;
    }

    public void ShowControls(bool toggle)
    {
        controlsUI.SetActive(toggle);
        GameManager.Instance.PauseGame(toggle);
        Blur(toggle);

        if(toggle)
        {
            GameManager.Instance.FreezeTime();
            GameManager.Instance.FreeMouse();
        }
        else
        {
            GameManager.Instance.NormalTime();
            GameManager.Instance.LockMouse();
        }
    }

    public void ToggleNotebookAlert(bool toggle)
    {
        archivesUnlocked.SetActive(toggle);
    }
}