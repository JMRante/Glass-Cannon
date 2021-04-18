using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public float reloadTime = 1.5f;
    private float reload = 0f;

    public Camera playerCamera;
    public Image crosshair;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Mathf.Approximately(reload, 0f))
        {
            crosshair.color = new Color(crosshair.color.r, crosshair.color.g, crosshair.color.b, 0.2f);
            reload = reloadTime;

            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, 500f))
            {
                if (hit.transform.name.StartsWith("balloon"))
                {
                    hit.transform.gameObject.SendMessage("Replace");
                }
            }
        }

        if (reload > 0f)
        {
            reload -= Time.deltaTime;
            
            if (reload <= 0f)
            {
                crosshair.color = new Color(crosshair.color.r, crosshair.color.g, crosshair.color.b, 1f);
                reload = 0f;
            }
        }
    }
}
