using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEndBalloon : MonoBehaviour
{
    public float waitTime = 2f;

    private StageBuilder stageBuilder;

    void Start()
    {
        stageBuilder = GameObject.Find("stage").GetComponent<StageBuilder>();
    }

    public void OnShot()
    {
        StartCoroutine(WaitThenGenerate());
    }

    IEnumerator WaitThenGenerate()
    {
        yield return new WaitForSeconds(waitTime);
        stageBuilder.Generate();
    }
}
