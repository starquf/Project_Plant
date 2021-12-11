using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    public List<Text> fruitTxts = new List<Text>();
    private Dictionary<FruitType, Text> fruitUIDic;

    public Button pauseBtn;
    public CanvasGroup pauseGroup;

    [Header("¹öÆ°µé")]
    public Button returnBtn;
    public Button retryBtn;
    public Button homeBtn;

    private void Start()
    {
        Init();

        GameManager.Instance.onUpdateUI += UpdateUI;

        pauseBtn.onClick.AddListener(() => Pause(true));

        returnBtn.onClick.AddListener(() => Pause(false));
        retryBtn.onClick.AddListener(() => GameManager.Instance.Restart());
        homeBtn.onClick.AddListener(() => GameManager.Instance.LoadScene("Title"));

        Pause(false);
    }

    private void Pause(bool enable)
    {
        pauseGroup.blocksRaycasts = enable;
        pauseGroup.interactable = enable;
        pauseGroup.alpha = enable ? 1 : 0;

        GameManager.Instance.tileHandler.canInteract = !enable;
    }

    private void Init()
    {
        fruitUIDic = new Dictionary<FruitType, Text>();

        fruitUIDic.Add(FruitType.WATERMELON, fruitTxts[0]);
        fruitUIDic.Add(FruitType.APPLE, fruitTxts[1]);
        fruitUIDic.Add(FruitType.BANANA, fruitTxts[2]);
    }

    private void UpdateUI()
    {
        int value = 0;

        foreach (var fruitType in fruitUIDic.Keys)
        {
            value = GameManager.Instance.fruitCountDic[fruitType];
            print($"{fruitType} : {value}");

            if (value < 0)
            {
                value = 0;
            }

            fruitUIDic[fruitType].text = value.ToString();
        }
    }
}
