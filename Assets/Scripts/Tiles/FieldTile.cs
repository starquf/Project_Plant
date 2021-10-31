using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FieldTile : Tile
{
    public Transform plant;
    private SpriteRenderer plantSr;

    private NeedType needs;

    private void Start()
    {
        plantSr = plant.GetComponent<SpriteRenderer>();
        plantSr.enabled = false;

        plant.localScale = Vector3.zero;

        GameManager.Instance.onGameClear += Bloom;
    }

    private void Bloom()
    {
        plantSr.enabled = true;
        plant.DOScale(Vector3.one, 0.75f)
            .SetEase(Ease.OutBack);

    }

    public void GetWater()
    {
        GameManager.Instance.GameClear();
    }
}
