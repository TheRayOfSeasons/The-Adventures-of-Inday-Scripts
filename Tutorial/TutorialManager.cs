using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

    [Header("Movement")]
    public bool movementTestDone = false;
    public bool up_press = false;
    public bool left_press = false;
    public bool back_press = false;
    public bool right_press = false;
    public bool jump_press = false;
    public bool crouch_press = false;

    [Header("Inventory")]
    public bool inventoryTestDone = false;

    [Header("Spell Cast")]
    public bool spellCastTestDone = false;

    [Header("Archives")]
    public bool archiveTestDone = false;

    void Start()
    {
        bool tutorial = !GameManager.Instance.Tutorial;

        movementTestDone = tutorial;
        up_press = tutorial;
        left_press = tutorial;
        back_press = tutorial;
        right_press = tutorial;
        jump_press = tutorial;
        crouch_press = tutorial;
        inventoryTestDone = tutorial;
        spellCastTestDone = tutorial;
        archiveTestDone = tutorial;
    }

    void Update()
    {
        if(!GameManager.Instance.Tutorial)
            return;

        if(!movementTestDone)
        {
            MovementTest();
        }
        else
        {
            if(!inventoryTestDone)
            {
                InventoryTest();
            }
            else
            {
                if(!archiveTestDone)
                {
                    ArchiveTest();
                }
                else
                {
                    if(!spellCastTestDone)
                    {
                        SpellCastTest();
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }

    void MovementTest()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(up_press)
                return;

            up_press = true;
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(left_press)
                return;

            left_press = true;
        }

        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(back_press)
                return;

            back_press = true;
        }

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(right_press)
                return;

            right_press = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(jump_press)
                return;

            jump_press = true;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(crouch_press)
                return;

            crouch_press = true;
        }

        if(AllMovementTestsDone())
            movementTestDone = true;
    }

    void InventoryTest()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryTestDone)
                return;
            
            inventoryTestDone = true;
        }
    }

    void ArchiveTest()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if(archiveTestDone)
                return;

            archiveTestDone = true;
        }
    }

    void SpellCastTest()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(spellCastTestDone)
                return;

            spellCastTestDone = true;
        }
    }

    private bool AllMovementTestsDone()
    {
        foreach(bool i in new bool[]{up_press, left_press,back_press, right_press, jump_press, crouch_press})
        {
            if(!i)
                return false;
        }

        return true;
    }

    public bool AllTestsDone()
    {
        foreach(bool i in new bool[]{movementTestDone, inventoryTestDone, archiveTestDone, spellCastTestDone})
        {
            if(!i)
                return false;
        }

        return true;
    }
}
