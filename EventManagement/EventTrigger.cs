using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : EventManager
{
    [SerializeField] private List<GameObject> ObjectsToActivate;
    [SerializeField] private EventSelector _event;
    [SerializeField] private bool activate;
    [SerializeField] private bool disableOnTrigger;
    [SerializeField] private bool pauseOnTrigger;
    [SerializeField] private bool triggerByKeyPress;
    [SerializeField] private bool isUIType;

    void OnTriggerEnter(Collider other)
    {
        if(triggerByKeyPress)
            return;

        if(other.tag == Tags.PLAYER)
        {
            GetDelegate(_event)();

            if(ObjectsToActivate.Count > 0)
                foreach(GameObject go in ObjectsToActivate)
                    go.SetActive(activate);

            if(pauseOnTrigger)
                GameManager.Instance.PauseGame(true);

            if(disableOnTrigger)
                gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!triggerByKeyPress)
            return;

        if(isUIType)
            if(!UIManager.Instance.CanOpenNewUIPanel())
                return;

        if(other.tag == Tags.PLAYER)
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                GetDelegate(_event)();

                if(GetComponent<DialogSequence>())
                {
                    if(!GetComponent<DialogSequence>().NoLongerTriggerable)
                        GetComponent<DialogSequence>().InitiateDialog();
                }

                if(ObjectsToActivate.Count > 0)
                    foreach(GameObject go in ObjectsToActivate)
                        go.SetActive(activate);

                if(pauseOnTrigger)
                    GameManager.Instance.PauseGame(true);

                if(disableOnTrigger)
                    gameObject.SetActive(false);

                if(GetComponent<InteractableUI>())
                    UIManager.Instance.DisableInteractionText();
            }
        }
    }
}
