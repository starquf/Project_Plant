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
    }
    #endregion

    public event Action onRefresh;
    public event Action onRefreshAfter;
    public event Action onGameClear;

    [HideInInspector]
    public TileHandler tileHandler = null;


    public void OnRefresh()
    {
        onRefresh?.Invoke();
        onRefreshAfter?.Invoke();
    }

    public void GameClear()
    {
        onGameClear?.Invoke();
    }

    public void Restart()
    {
        onRefresh = null;
        onRefreshAfter = null;
        onGameClear = null;

        DOTween.KillAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
