using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceOnShot : MonoBehaviour
{
    public GameObject replacementPrefab;

    public void OnShot()
    {
        Instantiate(replacementPrefab, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
