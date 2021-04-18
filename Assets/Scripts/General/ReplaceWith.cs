using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceWith : MonoBehaviour
{
    public GameObject replacementPrefab;

    public void Replace()
    {
        Instantiate(replacementPrefab, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
