using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;

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
                    Debug.LogError("게임메니져가 존재하지 않습니다!");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)                       // 만약 instance가 비어있다면
        {
            instance = this;
        }
        else if (instance != this)                   // 비어있진 않은데 instance가 자신이 아니라면
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(this.gameObject);

        fruitCountDic.Add(FruitType.WATERMELON, 0);
        fruitCountDic.Add(FruitType.APPLE, 0);
        fruitCountDic.Add(FruitType.BANANA, 0);

        Init();
    }
    #endregion

    private readonly string fileName = "gameinfo.txt";
    private string path;
    public GameInfoVO gameInfo;

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

    public bool isSceneChanging = false;

    private void Init()
    {
        Screen.SetResolution(1920, 1080, true);

        gameInfo = new GameInfoVO();

        path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            LoadGameInfo();
        }
        else
        {
            SaveGameInfo();
        }
    }

    

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
        LoadScene("InGame");
    }

    private void ResetScene()
    {
        onRefresh = null;
        onRefreshAfter = null;
        onCheckClear = null;
        onGameClear = null;
        onUpdateUI = null;

        PoolManager.ResetPool();
        DOTween.KillAll();
    }

    public void LoadScene(string sceneName)
    {
        isSceneChanging = true;
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

                yield return null;

                loadImg.blocksRaycasts = false;
                loadImg.DOFade(0f, changeDur - 0.1f)
                    .SetEase(Ease.Linear);

                isSceneChanging = false;

                yield break;
            }
        }
    }

    public void LoadGameInfo()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            gameInfo = JsonUtility.FromJson<GameInfoVO>(json);
        }
    }

    [ContextMenu("현재 정보 저장하기")]
    public void SaveGameInfo()
    {
        string json = JsonUtility.ToJson(gameInfo);

        File.WriteAllText(path, json);
    }

    [ContextMenu("현재 정보 초기화 (주의)")]
    public void ResetGameInfo()
    {
        gameInfo = new GameInfoVO();
        SaveGameInfo();
    }
}
