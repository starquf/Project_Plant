using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterLineTile : Tile
{
    private LayerMask whatIsTile;

    private SpriteRenderer sr;
    private List<SpriteRenderer> lines = new List<SpriteRenderer>();

    private Color waterColor;

    [Header("현재 방향")]
    public WaterLineDir myDir = WaterLineDir.None;
    [SerializeField] private WaterLineDir currentDir = WaterLineDir.None;

    private Dictionary<Vector3, WaterLineDir> dicDir = new Dictionary<Vector3, WaterLineDir>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.GetChild(0).GetComponentsInChildren(lines);

        whatIsTile = LayerMask.GetMask("TILE");

        waterColor = lines[0].color;

        GameManager.Instance.onRefresh += Refresh;
        Refresh();

        dicDir.Add(Vector3.left, WaterLineDir.Left);
        dicDir.Add(Vector3.right, WaterLineDir.Right);
        dicDir.Add(Vector3.up, WaterLineDir.Up);
        dicDir.Add(Vector3.down, WaterLineDir.Down);
    }

    private void Refresh()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].color = Color.gray;
        }

        currentDir = myDir;
    }

    public void GetWater(Vector3 getDir)
    {
        WaterLineDir lineDir = GetWaterDir(getDir * -1f);

        // 만약 물은 받을 수 없다면
        if (!currentDir.HasFlag(lineDir))
        {
            return;
        }

        // 물을 받은 상태
        currentDir &= ~lineDir;

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].color = waterColor;
        }

        List<Vector3> giveDir = CheckDir();

        // 물을 줄 수 있는 상태면
        if (giveDir.Count > 0)
        {
            Tile tile = null;

            for (int i = 0; i < giveDir.Count; i++)
            {
                tile = Physics2D.OverlapPoint(transform.position + giveDir[i], whatIsTile)
                    ?.GetComponent<Tile>();

                if (tile != null)
                {
                    switch (tile.tileType)
                    {
                        case TileType.WATERLINE:

                            tile.GetComponent<WaterLineTile>().GetWater(giveDir[i]);
                            currentDir &= ~GetWaterDir(giveDir[i]);

                            break;

                        case TileType.FIELD:

                            tile.GetComponent<FieldTile>().GetWater();
                            currentDir &= ~GetWaterDir(giveDir[i]);

                            break;
                    }
                }
            }
        }
    }

    #region Dir
    private WaterLineDir GetWaterDir(Vector3 dir)
    {
        if (!dicDir.ContainsKey(dir))
        {
            return WaterLineDir.None;
        }

        return dicDir[dir];
    }

    private List<Vector3> CheckDir()
    {
        List<Vector3> giveDir = new List<Vector3>();

        // 줄 수 있는 방향을 전부 확인
        foreach (var dir in dicDir.Keys)
        {
            if (currentDir.HasFlag(dicDir[dir]))
            {
                giveDir.Add(dir);
            }
        }

        return giveDir;
    }
    #endregion

    public override void OnTouch()
    {
        GameManager.Instance.tileHandler.ChangeTile(this);
    }

    public override void Select(bool isSelect)
    {
        if (isSelect)
        {
            sr.DOFade(0.5f, 0.15f).SetEase(Ease.Linear);

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].DOFade(0.5f, 0.15f).SetEase(Ease.Linear);
            }
        }
        else
        {
            sr.DOFade(1f, 0.15f).SetEase(Ease.Linear);

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].DOFade(1f, 0.15f).SetEase(Ease.Linear);
            }
        }
    }
}
