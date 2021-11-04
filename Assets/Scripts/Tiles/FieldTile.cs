using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FieldTile : Tile
{
    public Transform plant;
    private SpriteRenderer plantSr;

    private NeedType needs;

    private bool isBloom = false;

    private void Awake()
    {
        GameManager.Instance.maxField++;
    }

    private void Start()
    {
        plantSr = plant.GetComponent<SpriteRenderer>();
        plantSr.enabled = false;

        plant.localScale = Vector3.zero;

        GameManager.Instance.onRefresh += CancelBloom;
    }

    private void Bloom()
    {
        plantSr.enabled = true;
        plant.DOScale(Vector3.one, 0.75f)
            .SetEase(Ease.OutBack);

        isBloom = true;
    }

    private void CancelBloom()
    {
        if (isBloom)
            GameManager.Instance.maxField++;

        plantSr.enabled = false;
        plant.localScale = Vector3.zero;

        isBloom = false;
    }

    public void GetWater()
    {
        if (isBloom) return;

        GameManager.Instance.FieldActivate();

        Bloom();
    }
}
