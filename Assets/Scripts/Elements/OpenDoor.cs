using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator anim;
    private BoxCollider doorCollider;
    private float timeOpen = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        doorCollider = GetComponent<BoxCollider>();
    }

    public void OnShot()
    {
        StartCoroutine(WaitOpen());
    }

    IEnumerator WaitOpen()
    {
        doorCollider.enabled = false;
        anim.SetBool("isDoorOpen", true);
        yield return new WaitForSeconds(timeOpen);
        anim.SetBool("isDoorOpen", false);
        doorCollider.enabled = true;
    }
}
