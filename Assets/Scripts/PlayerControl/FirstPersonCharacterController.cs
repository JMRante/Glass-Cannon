using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float acceleration = 90f;
    public float friction = 90f;
    public float airAcceleration = 15f;
    
    private float gravity = -9.81f;

    private Vector3 velocity;
    private Vector3 moveVelocity;

    private CharacterController characterController;

    void Start()
    {
        velocity = Vector3.zero;
        moveVelocity = Vector3.zero;

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 force = Vector3.zero;

        if (Mathf.Approximately(xInput, 0) && Mathf.Approximately(yInput, 0))
        {
            force = -moveVelocity.normalized * friction;
        }
        else
        {
            force = Vector3.Normalize(((transform.right * xInput) + (transform.forward * yInput))) * (characterController.isGrounded ? acceleration : airAcceleration);
        }

        moveVelocity += force * Time.deltaTime;
        moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxSpeed);

        characterController.Move(moveVelocity * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
