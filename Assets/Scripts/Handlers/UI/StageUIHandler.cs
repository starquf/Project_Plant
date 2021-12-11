using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIHandler : MonoBehaviour
{
    private List<Button> stageBtns;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        stageBtns = new List<Button>();
        GetComponentsInChildren(stageBtns);

        for(int i = 0; i < stageBtns.Count; i++)
        {
            int a = i;

            stageBtns[i].GetComponentInChildren<Text>().text = (a + 1).ToString();
            stageBtns[i].onClick.AddListener(() =>
            {
                GameManager.Instance.stageIdx = a;
                GameManager.Instance.LoadScene("InGame");
            });
        }
    }
}
