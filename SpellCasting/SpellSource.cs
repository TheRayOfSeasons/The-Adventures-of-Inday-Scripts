using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSource : MonoBehaviour
{
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Player player;

    private bool inShootMode = false;

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1))
        {
            if(GameManager.Instance.IsKeyboardEnabled())
                return;

            if(SpellManager.Instance.currentSpell == null)
                return;

            if(!SpellManager.Instance.currentSpell.isChargesType)
                return;

            if(!GameManager.Instance.CanAttack)
                return;

            if(!inShootMode)
            {
                inShootMode = true;
                cam.Zoom();
                GameManager.Instance.EnablePlayerMovement(false);
                GameManager.Instance.StopPlayerMovement(true);
                UIManager.Instance.ToggleCrosshair(true);
            }

            player.LookForward();
            if(Input.GetKeyDown(KeyCode.Mouse0))
                SpellManager.Instance.currentSpellDelegate();
        }
        else
        {
            if(inShootMode)
            {
                inShootMode = false;
                cam.NormalDistance();
                GameManager.Instance.EnablePlayerMovement(true);
                GameManager.Instance.StopPlayerMovement(false);
                UIManager.Instance.ToggleCrosshair(false);
            }
        }
    }
}
