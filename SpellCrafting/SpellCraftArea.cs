using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCraftArea : MonoBehaviour
{
    public bool unlocked = false;
    [SerializeField] private SpellShard shard;

    private DialogSequence sequence;

    void Start()
    {
        if(GetComponent<DialogSequence>())
            sequence = GetComponent<DialogSequence>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.PLAYER)
            EventManager.currentSpellCraftArea = this;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == Tags.PLAYER)
        {
            if(Input.GetKeyUp(KeyCode.E) && !unlocked)
                SpellCraftManager.Instance.Craft(shard);
        }
    }

    public void InitiateUnlockDialog()
    {
        if(!sequence)
            return;

        sequence.InitiateDialog();
        DialogManager.Instance.UnloadDialog();
    }
}
