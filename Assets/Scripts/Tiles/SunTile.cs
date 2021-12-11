using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTile : Tile
{
    private LineRenderer lr;

    private LayerMask whatIsTile;
    private float distance;

    protected void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.SetPosition(0, transform.position + transform.rotation * Vector3.right * 0.45f);
        lr.SetPosition(1, transform.position);

        whatIsTile = LayerMask.GetMask("TILE");
        distance = Mathf.Abs(Camera.main.ViewportToWorldPoint(Vector2.zero).x) 
            + Mathf.Abs(Camera.main.ViewportToWorldPoint(Vector2.one).x);

        GameManager.Instance.onRefreshAfter += GiveSun;
        GiveSun();
    }

    private void GiveSun()
    {
        lr.positionCount = 2;

        lr.SetPosition(0, transform.position + transform.rotation * Vector3.right * 0.45f);

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.rotation * Vector2.right, distance, whatIsTile);

        if (hit == null) return;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform != null)
            {
                Tile tile = hit[i].transform.GetComponent<Tile>();

                if (tile.tileType.Equals(TileType.MIRROR))
                {
                    lr.SetPosition(1, tile.transform.position);

                    MirrorTile mirrorTile = tile as MirrorTile;
                    mirrorTile.ReflectSun(lr, transform.rotation * Vector2.right, 999);

                    return;
                }

                if (tile.tileType.Equals(TileType.FIELD))
                {
                    lr.SetPosition(1, tile.transform.position);

                    FieldTile fieldTile = tile as FieldTile;
                    fieldTile.GetSun();
                }
            }
        }

        lr.SetPosition(1, transform.position + transform.rotation * Vector2.right * distance);
    }
}
