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
