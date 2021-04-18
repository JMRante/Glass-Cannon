using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float acceleration = 0.2f;
    public float friction = 0.1f;

    private Vector3 velocity;

    void Start()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 force = Vector3.zero;

        if (Mathf.Approximately(xInput, 0) && Mathf.Approximately(yInput, 0))
        {
            force = -velocity.normalized * friction;
        }
        else
        {
            force = Vector3.Normalize(((Vector3.right * xInput) + (Vector3.forward * yInput))) * acceleration;
        }

        velocity += force;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.Translate(velocity * Time.deltaTime);
    }
}
