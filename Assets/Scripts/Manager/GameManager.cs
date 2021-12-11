using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region SingleTon
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    Debug.LogError("���Ӹ޴����� �������� �ʽ��ϴ�!");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)                       // ���� instance�� ����ִٸ�
        {
            instance = this;
        }
        else if (instance != this)                   // ������� ������ instance�� �ڽ��� �ƴ϶��
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(this.gameObject);

        fruitCountDic.Add(FruitType.WATERMELON, 0);
        fruitCountDic.Add(FruitType.APPLE, 0);
        fruitCountDic.Add(FruitType.BANANA, 0);
    }
    #endregion

    public CanvasGroup loadImg;
    private float changeDur = 1f;

    public event Action onRefresh;
    public event Action onRefreshAfter;
    public event Action onCheckClear;
    public event Action onUpdateUI;

    public event Action onGameClear;

    [HideInInspector]
    public TileHandler tileHandler = null;

    public Dictionary<FruitType, int> fruitCountDic = new Dictionary<FruitType, int>();

    public int waterMelonCnt = 0;
    public int appleCnt = 0;
    public int bananaCnt = 0;

    public int stageIdx = 0;

    public void OnRefresh()
    {
        fruitCountDic[FruitType.WATERMELON] = waterMelonCnt;
        fruitCountDic[FruitType.APPLE] = appleCnt;
        fruitCountDic[FruitType.BANANA] = bananaCnt;

        onRefresh?.Invoke();
        onRefreshAfter?.Invoke();
        onCheckClear?.Invoke();
        onUpdateUI?.Invoke();
    }

    public void SetFruitCnt(int waterMelonCnt, int appleCnt, int bananaCnt)
    {
        this.waterMelonCnt = waterMelonCnt;
        this.appleCnt = appleCnt;
        this.bananaCnt = bananaCnt;
    }

    public void FieldActivate(FruitType fruitType)
    {
        if (fruitType.Equals(FruitType.NONE)) return;

        fruitCountDic[fruitType]--;

        if (CheckClear())
        {
            onGameClear?.Invoke();
        }
    }

    private bool CheckClear()
    {
        bool gameClear = true;

        foreach (int count in fruitCountDic.Values)
        {
            //print(count);

            if (count > 0)
            {
                gameClear = false;
            }
        }

        return gameClear;
    }

    public void Restart()
    {
        print("�����");
        ResetScene();

        LoadScene("InGame");
    }

    private void ResetScene()
    {
        onRefresh = null;
        onRefreshAfter = null;
        onCheckClear = null;
        onGameClear = null;
        onUpdateUI = null;

        DOTween.KillAll();
    }

    public void LoadScene(string sceneName)
    {
        loadImg.blocksRaycasts = true;

        loadImg.DOFade(1f, changeDur)
            .OnComplete(() =>
            {
                ResetScene();
                StartCoroutine(Loading(sceneName));
            })
            .SetEase(Ease.Linear);
    }


    IEnumerator Loading(string sceneName)
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;

                loadImg.blocksRaycasts = false;
                loadImg.DOFade(0f, changeDur - 0.1f)
                    .SetEase(Ease.Linear);

                yield break;
            }
        }
    }
}
