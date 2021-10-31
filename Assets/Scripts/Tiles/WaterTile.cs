using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : Tile
{
    private LayerMask whatIsTile;

    private List<Vector3> dirs = new List<Vector3>();

    private void Awake()
    {
        isClickable = false;
        whatIsTile = LayerMask.GetMask("TILE");

        dirs.Add(Vector3.left);
        dirs.Add(Vector3.right);
        dirs.Add(Vector3.up);
        dirs.Add(Vector3.down);
    }

    protected void Start()
    {
        GameManager.Instance.onRefreshAfter += GiveWater;

        GiveWater();
    }

    private void GiveWater()
    {
        // 물을 주는 것
        for (int i = 0; i < dirs.Count; i++)
        {
            WaterLineTile tile = Physics2D.OverlapPoint(transform.position + dirs[i], whatIsTile)
                ?.GetComponent<WaterLineTile>();

            if (tile != null)
            {
                tile.GetWater(dirs[i]);
            }
        }
    }
}
