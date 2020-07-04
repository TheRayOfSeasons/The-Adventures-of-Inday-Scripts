using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public bool isDialogRunning = false;
    public bool canUnload = true;

    private static DialogManager instance;
    public static DialogManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private GameObject dialogPanel;
    public GameObject DialogPanel
    {
        get { return dialogPanel; }
    }
    [SerializeField] private Image dialogImage;
    [SerializeField] private Text dialogName;
    [SerializeField] private Text dialogMessage;
    [SerializeField] private Dialog dialog;
    [SerializeField] private DialogSequence dialogSequence;

    [SerializeField] private Sprite Inday;
    [SerializeField] private Sprite ShopKeep;
    [SerializeField] private Sprite Guardian;

    public Dialog currentDialog
    {
        set { dialog = value; }
        get { return dialog; }
    }

    // Queue
    private Queue<string> names;
    private Queue<string> messages;
    private Queue<Image> icons;
    private Sprite defaultImage;

    private int dialogQueueCount;
    public int DialogQueueCount
    {
        set { dialogQueueCount = value; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        names = new Queue<string>();
        messages = new Queue<string>();
        // icons = new Queue<Image>();
    }

    void Update()
    {
        if(GameManager.Instance.totalPause)
            return;
            
        if(GameManager.Instance.PlayerInGameInputDisabled || !TutorialManager.Instance.AllTestsDone())
            if (Input.GetKeyUp(KeyCode.E) && DialogManager.Instance.isDialogRunning && canUnload)
                UnloadDialog();
    }

    public void InitiateDialog(Dialog dialog, DialogSequence sequenceScript)
    {
        isDialogRunning = true;
        EnableUI(true);
        currentDialog = dialog;
        dialogSequence = sequenceScript;
        UIManager.Instance.DisableUIInteraction(true);
        UIManager.Instance.Blur(true);
        GameManager.Instance.StopPlayerMovement(true);
        GameManager.Instance.PauseUIInputs(true);
        GameManager.Instance.PauseEnemies(true);
        GameManager.Instance.MouseMovementActive = false;

        foreach(string name in dialog.names)
            names.Enqueue(name);

        foreach(string message in dialog.messages)
            messages.Enqueue(message);


        // foreach(Image icon in currentDialog.icons)
        //     icons.Enqueue(icon);

        if(dialog.defaultPortrait)
            defaultImage = dialog.defaultPortrait;
    }

    public void EnableUI(bool toggle)
    {
        dialogPanel.SetActive(toggle);
        UIManager.Instance.ToggleMainUI(!toggle);
    }

    public void UnloadDialog()
    {
        if (dialog != null)
        {
            DisplayNext();
        }
    }

    public void DisplayNext()
    {
        if (messages.Count == 0)
        {
            EndDialog();
            return;
        }

        string name = names.Dequeue();
        string message = messages.Dequeue();
        // Image image = icons.Dequeue();

        dialogName.text = name;
        dialogMessage.text = message;
        // dialogImage.image = image;

        switch(name)
        {
            case "Inday":
                dialogImage.sprite = Inday;
                break;
            case "Mysterious Shopkeeper":
                dialogImage.sprite = ShopKeep;
                break;
            case "Guardian":
                dialogImage.sprite = Guardian;
                break;
            default:
                if(defaultImage)
                    dialogImage.sprite = defaultImage;
                break;
        }
        
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        dialogMessage.text = "";

        foreach (char letter in message.ToCharArray())
        {
            dialogMessage.text += letter;
            yield return null;
        }
    }

    public void EndDialog()
    {
        dialog = null;
        EnableUI(false);
        isDialogRunning = false;
        dialogSequence.DoAfterDialogEvent();
        UIManager.Instance.DisableUIInteraction(false);
        GameManager.Instance.StopPlayerMovement(false);

        if(UIManager.Instance.HasInteractionText)
            UIManager.Instance.DisableInteractionText();
    }
}
