using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FieldTile : Tile
{
    public Transform plant;
    private SpriteRenderer plantSr;

    private FruitType fruitType;

    private NeedType currentNeed = NeedType.None;

    public List<Sprite> fruitSprs = new List<Sprite>();

    private Tween plantTween;

    private void Awake()
    {
        plantSr = plant.GetComponent<SpriteRenderer>();

        plantSr.enabled = false;
        plant.localScale = Vector3.zero;
    }

    private void Start()
    {
        GameManager.Instance.onRefresh += CancelBloom;
        GameManager.Instance.onCheckClear += CheckBloom;
    }

    private void Bloom(FruitType fruitType)
    {
        plantSr.enabled = true;

        plantTween.Kill();
        plantTween = plant.DOScale(Vector3.one, 0.75f)
                        .SetEase(Ease.OutBack);

        switch (fruitType)
        {
            case FruitType.WATERMELON:
                plantSr.sprite = fruitSprs[0];
                break;

            case FruitType.APPLE:
                plantSr.sprite = fruitSprs[1];
                break;

            case FruitType.BANANA:
                plantSr.sprite = fruitSprs[2];
                break;
        }
    }

    private void CancelBloom()
    {
        plantSr.enabled = false;
        plant.localScale = Vector3.zero;

        fruitType = FruitType.NONE;
        currentNeed = NeedType.None;
    }

    public void GetWater()
    {
        if (currentNeed.HasFlag(NeedType.WATER)) return;

        currentNeed |= NeedType.WATER;
    }

    public void GetSun()
    {
        if (currentNeed.HasFlag(NeedType.SUN)) return;

        currentNeed |= NeedType.SUN;
    }

    private void CheckBloom()
    {
        if (currentNeed.HasFlag(NeedType.WATER))
        {
            fruitType = FruitType.WATERMELON;

            if (currentNeed.HasFlag(NeedType.SUN))
            {
                fruitType = FruitType.BANANA;
            }
        }
        else if (currentNeed.HasFlag(NeedType.SUN))
        {
            fruitType = FruitType.APPLE;
        }

        if (!fruitType.Equals(FruitType.NONE))
        {
            GameManager.Instance.FieldActivate(fruitType);
            Bloom(fruitType);
        }
    }
}
