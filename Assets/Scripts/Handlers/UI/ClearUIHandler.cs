using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearUIHandler : MonoBehaviour
{
    private CanvasGroup cg;

    public Button retryBtn;
    public Button nextBtn;
    public Button homeBtn;

    public List<ParticleSystem> fireworks = new List<ParticleSystem>();

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;

        GameManager.Instance.onGameClear += GameClear;

        retryBtn.onClick.AddListener(() => GameManager.Instance.Restart());
        homeBtn.onClick.AddListener(() => GameManager.Instance.LoadScene("Title"));
        nextBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.stageIdx++;
            GameManager.Instance.Restart();
        });


        if (GameManager.Instance.stageIdx >= 14)
        {
            nextBtn.gameObject.SetActive(false);
        }
    }

    private void GameClear()
    {
        for (int i = 0; i < fireworks.Count; i++)
        {
            fireworks[i].Play();
        }

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
