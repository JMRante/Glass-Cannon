using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBob : MonoBehaviour
{
    public float bobRange = 0.1f;

    private float startPoint;
    private float startHeight;

    void Start()
    {
        startPoint = Random.Range(0f, Mathf.PI * 2);
        startHeight = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, startHeight + (Mathf.Sin(Time.time + startPoint) * bobRange), transform.position.z);
    }
}
