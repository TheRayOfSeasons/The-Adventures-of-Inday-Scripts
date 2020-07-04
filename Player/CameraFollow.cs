using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraZoomOffset;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float defaultDistance;
    [SerializeField] private float currentX;
    [SerializeField] private float currentY;

    private float distance;
    private float camX;

    private const float X_MIN_CLAMP = -50;
    private const float X_MAX_CLAMP = 50;

    void Start()
    {
        camX = cameraTransform.position.x;
        NormalDistance();
    }

    void Update()
    {
        if (!GameManager.Instance.IsPaused && GameManager.Instance.MouseMovementActive)
        {
            currentX += Input.GetAxis("Mouse Y") * Options.Instance.Data.MouseSensitivity;
            currentY += Input.GetAxis("Mouse X") * Options.Instance.Data.MouseSensitivity;

            currentX = Mathf.Clamp(currentX, X_MIN_CLAMP, X_MAX_CLAMP);
        }
    }

    void LateUpdate()
    {
        if(!GameManager.Instance.IsPaused)
        {
            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(-currentX, currentY, 0f);
            transform.position = Player.position + rotation * direction;
            transform.LookAt(Player.position);
        }
    }

    public void Zoom()
    {
        distance = zoomAmount;
        cameraTransform.localPosition = new Vector3(cameraZoomOffset, 0f, 0f);
    }

    public void Zoom(float zoom)
    {
        cameraTransform.localPosition = new Vector3(0f, 0f, zoom);
    }

    public void NormalDistance()
    {
        distance = defaultDistance;
        cameraTransform.localPosition = new Vector3(camX, 0f, 0f);
    }
}
