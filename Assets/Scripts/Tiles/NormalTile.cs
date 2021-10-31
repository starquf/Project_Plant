using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NormalTile : Tile
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnTouch()
    {
        GameManager.Instance.tileHandler.ChangeTile(this);
    }

    public override void Select(bool isSelect)
    {
        if (isSelect)
        {
            sr.DOFade(0.5f, 0.15f).SetEase(Ease.Linear);
        }
        else
        {
            sr.DOFade(1f, 0.15f).SetEase(Ease.Linear);
        }
    }
}
