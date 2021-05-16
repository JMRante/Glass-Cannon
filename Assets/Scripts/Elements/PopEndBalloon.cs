using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEndBalloon : MonoBehaviour
{
    public float waitTime = 2f;

    private StageBuilder stageBuilder;

    public GameObject replacementPrefab;

    private bool isShot;

    void Start()
    {
        stageBuilder = GameObject.Find("stage").GetComponent<StageBuilder>();
        isShot = false;
    }

    public void OnShot()
    {
        if (!isShot)
        {
            isShot = true;
            Instantiate(replacementPrefab, transform.position, transform.rotation);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(WaitThenGenerate());
        }
    }

    IEnumerator WaitThenGenerate()
    {
        yield return new WaitForSeconds(waitTime);
        stageBuilder.Generate();
    }
}
