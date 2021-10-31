using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    public Button retryBtn;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;

        GameManager.Instance.onGameClear += GameClear;

        retryBtn.onClick.AddListener(() => GameManager.Instance.Restart());
    }

    private void GameClear()
    {
        Sequence seq = DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                cg.blocksRaycasts = true;
                cg.interactable = true;
            })
            .Append(cg.DOFade(1f, 1f));
            //.Append();
    }
}
