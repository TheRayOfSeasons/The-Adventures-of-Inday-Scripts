using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogSequence : EventManager
{
    [SerializeField] private bool hasOnceOnlyDialog;
    [SerializeField] private bool EndsWithUI;
    [SerializeField] private bool InitiateOnTrigger;
    [SerializeField] private bool Triggerable;
    [SerializeField] private bool onceOnly;
    [SerializeField] private bool doesNotManipulatePauseState;

    private bool noLongerTriggerable = false;

    public bool NoLongerTriggerable
    {
        get { return noLongerTriggerable; }
    }

    public List<Dialog> dialogs;
    [SerializeField] private List<EventSelector> events;

    [Header("If does not have once only dialog")]
    public List<Dialog> newDialogs;
    [SerializeField] private List<EventSelector> newEvents;

    private Queue<Dialog> dialogQueue;
    private Queue<EventSelector> eventQueue;
    
    public enum Actions
    {
        doNothing
    };

    void Start()
    {
        dialogQueue = new Queue<Dialog>();
        eventQueue = new Queue<EventSelector>();

        QueueOriginal();
    }

    public static DialogSequence Default()
    {
        DialogSequence dialogSeqeuence = new DialogSequence();
        dialogSeqeuence.SetEventSelector(EventSelector.doNothing);

        return dialogSeqeuence;
    }

    public void InitiateDialog()
    {
        DialogManager.Instance.InitiateDialog(dialogQueue.Dequeue(), this);
        // GameManager.Instance.PauseGame(true);
        GameManager.Instance.PausePlayerInput(true);

        if(transform.parent.GetComponent<Animator>())
            transform.parent.GetComponent<Animator>().SetBool("Talk", true);

        if(GetComponent<Shop>() != null)
        {
            GameManager.Instance.currentShopItems = GetComponent<Shop>().shopItems;
        }

        if(onceOnly)
            noLongerTriggerable = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!InitiateOnTrigger)
            return;

        if (other.gameObject.tag == Tags.PLAYER && !DialogManager.Instance.isDialogRunning)
        {
            if(!GameManager.Instance.PlayerInGameInputDisabled)
            {
                InitiateDialog();
                DialogManager.Instance.UnloadDialog();
                UIManager.Instance.DisableInteractionText();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(noLongerTriggerable)
            return;
            
        if(InitiateOnTrigger)
            return;
            
        if (other.gameObject.tag == Tags.PLAYER && !DialogManager.Instance.isDialogRunning)
        {
            if(!GameManager.Instance.PlayerInGameInputDisabled)
            {
                if (Input.GetKeyDown(KeyCode.E) && Triggerable)
                {
                    InitiateDialog();
                    AudioManager.Instance.access.Play();
                    UIManager.Instance.DisableInteractionText();
                }
            }
        }
    }

    public void SetEventSelector(EventSelector select)
    {
        eventQueue = new Queue<EventSelector>();
        eventQueue.Enqueue(select);
    }

    public void DoAfterDialogEvent()
    {
        EventManager.GetDelegate(eventQueue.Dequeue())();

        if(transform.parent.GetComponent<Animator>())
            transform.parent.GetComponent<Animator>().SetBool("Talk", false);

        if(dialogQueue.Count > 0)
        {
            InitiateDialog();
            DialogManager.Instance.UnloadDialog();
        }
        else
        {
            if(!doesNotManipulatePauseState)
            {
                GameManager.Instance.PausePlayerInput(EndsWithUI);
                GameManager.Instance.PauseUIInputs(false);
                GameManager.Instance.PauseEnemies(false);
                GameManager.Instance.MouseMovementActive = true;

                if(!EndsWithUI)
                {

                    GameManager.Instance.PauseGame(false);
                    UIManager.Instance.Blur(false);

                    if(EventManager.currentSpellCraftArea)
                        EventManager.currentSpellCraftArea = null;
                }
                else
                {
                    UIManager.Instance.ToggleMainUI(false);
                }
            }

            UIManager.Instance.Blur(false);
            ReQueueDialog();
        }
    }

    private void ReQueueDialog()
    {
        if(hasOnceOnlyDialog)
        {
            foreach(Dialog dialog in newDialogs)
                dialogQueue.Enqueue(dialog);

            foreach (EventSelector e in newEvents)
                eventQueue.Enqueue(e);
        }
        else
        {
            QueueOriginal();
        }
    }

    private void QueueOriginal()
    {
        foreach(Dialog dialog in dialogs)
            dialogQueue.Enqueue(dialog);

        foreach (EventSelector e in events)
            eventQueue.Enqueue(e);
    }
}
