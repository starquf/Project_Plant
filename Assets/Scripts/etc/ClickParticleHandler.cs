using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticleHandler : MonoBehaviour
{
    public GameObject particleObj;

    private Camera mainCam;

    void Start()
    {
        PoolManager.CreatePool<ClickParticle>(particleObj, transform);

        mainCam = Camera.main;
    }

    private void Update()
    {
        GetClick();
    }

    private void GetClick()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.isSceneChanging)
        {
            PoolingObj obj = PoolManager.GetItem<ClickParticle>();

            Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            obj.transform.position = pos;
        }
    }
}
