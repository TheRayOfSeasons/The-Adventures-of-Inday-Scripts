using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool alert;
    [SerializeField] private bool playAudio;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if(triggered)
            return;

        if(other.tag == Tags.PLAYER)
        {
            GameManager.Instance.checkpoint = this.transform;
            triggered = true;

            if(playAudio)
                AudioManager.Instance.checkpoint.Play();
            if(alert)
                UIManager.Instance.Alert("Checkpoint Reached", 2.5f);

            other.gameObject.GetComponent<Player>().RestoreToFull();
            other.gameObject.GetComponent<SlingShot>().ResetSlingMode();
        }
    }
}
