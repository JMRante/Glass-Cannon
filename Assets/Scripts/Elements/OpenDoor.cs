using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator anim;
    private BoxCollider doorCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        doorCollider = GetComponent<BoxCollider>();
    }

    public void OnShot()
    {
        doorCollider.enabled = false;
        anim.SetBool("isDoorOpen", true);
    }
}
