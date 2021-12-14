using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObj : MonoBehaviour
{
    public float showTime;
    private WaitForSeconds showWait;

    private void Awake()
    {
        showWait = new WaitForSeconds(showTime);
    }

    private void OnEnable()
    {
        StartCoroutine(DisableObj());
    }

    private IEnumerator DisableObj()
    {
        yield return showWait;

        gameObject.SetActive(false);
    }
}
