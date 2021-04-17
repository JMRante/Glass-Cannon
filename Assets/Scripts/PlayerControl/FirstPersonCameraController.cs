using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float mouseSensitivityInput = 0.2f;
    private const float mouseSensitivityMin = 1f;
    private const float mouseSensitivityMax = 10f;
    private float mouseSensitivity = mouseSensitivityMin;

    public bool invert = false;

    public float mouseVerticalRotateLimit = 90f;

    private float angleX;
    private float angleY;

    public Transform cameraTransform;

    CursorLockMode lockMode;

    void Awake()
    {
        lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }

    void Start()
    {
        mouseSensitivity = (mouseSensitivityInput * (mouseSensitivityMax - mouseSensitivityMin)) + mouseSensitivityMin;

        angleX = cameraTransform.eulerAngles.x;
        angleY = transform.eulerAngles.y;
    }

    void Update()
    {
        angleY += Input.GetAxis("Mouse X") * mouseSensitivity;
        angleY = ClampAngle(angleY, -360f, 360f);
        Quaternion playerRotation = Quaternion.Euler(0f, angleY, 0f);
        transform.rotation = playerRotation;

        angleX += Input.GetAxis("Mouse Y") * mouseSensitivity * (invert ? 1f : -1f);
        angleX = ClampAngle(angleX, -mouseVerticalRotateLimit, mouseVerticalRotateLimit);
        Quaternion cameraRotation = Quaternion.Euler(angleX, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
        cameraTransform.rotation = cameraRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }
        
        return Mathf.Clamp(angle, min, max);
    }
}
