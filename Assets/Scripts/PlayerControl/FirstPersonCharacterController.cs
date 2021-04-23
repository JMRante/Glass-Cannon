using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float acceleration = 75f;
    public float friction = 75f;
    public float airAcceleration = 15f;
    
    private float gravity = -9.81f;
    
    private float moveSpeed = 0f;
    private float fallSpeed = 0f;

    private Vector3 direction;

    private CharacterController characterController;

    void Start()
    {
        direction = Vector3.zero;

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded && fallSpeed < 0)
        {
            fallSpeed = 0f;
        }

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Mathf.Approximately(xInput, 0) && Mathf.Approximately(yInput, 0) && moveSpeed > 0f)
        {
            moveSpeed -= friction * Time.deltaTime;
        }
        else
        {
            moveSpeed += acceleration * Time.deltaTime;
            direction = Vector3.Normalize((transform.right * xInput) + (transform.forward * yInput));
        }

        moveSpeed = Mathf.Clamp(moveSpeed, 0f, 15f);

        characterController.Move(direction * moveSpeed * Time.deltaTime);

        fallSpeed += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * fallSpeed * Time.deltaTime);
    }
}
