using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float SpinAmount;

    void Update()
    {
        if(GameManager.Instance.IsPaused)
            return;
            
        transform.localEulerAngles = new Vector3 (
			transform.localEulerAngles.x,
			transform.localEulerAngles.y + SpinAmount,
			transform.localEulerAngles.z
		);
    }
}
