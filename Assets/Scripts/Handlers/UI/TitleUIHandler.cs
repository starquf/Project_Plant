using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIHandler : MonoBehaviour
{
    public Transform movePanel;

    [Header("¹öÆ°µé")]
    public Button startBtn;
    public Button creditBtn;
    public Button exitBtn;

    public List<Button> goBackBtns = new List<Button>();

    private void Start()
    {
        for (int i = 0; i < goBackBtns.Count; i++)
        {
            goBackBtns[i].onClick.AddListener(() =>
            {
                MovePanel(0f);
            });
        }

        startBtn.onClick.AddListener(() => 
        {
            MovePanel(-1920f);
        });

        creditBtn.onClick.AddListener(() =>
        {
            MovePanel(1920f);
        });

        exitBtn.onClick.AddListener(() => Application.Quit());
    }

    private void MovePanel(float x)
    {
        movePanel.DOLocalMoveX(x, 0.7f);
    }
}
