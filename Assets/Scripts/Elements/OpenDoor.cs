using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private BoxCollider doorCollider;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider>();
    }

    public void OnShot()
    {
        doorCollider.enabled = false;
    }
}
