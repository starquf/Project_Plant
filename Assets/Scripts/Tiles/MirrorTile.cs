using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MirrorTile : Tile
{
    private LayerMask whatIsTile;
    private float distance;

    int angleState = 1;
    Vector3 reflectDir;

    bool canRotate = true;

    private void Awake()
    {
        if (transform.eulerAngles.z.Equals(90f))
            angleState *= -1;

        whatIsTile = LayerMask.GetMask("TILE");
        distance = Mathf.Abs(Camera.main.ViewportToWorldPoint(Vector2.zero).x)
            + Mathf.Abs(Camera.main.ViewportToWorldPoint(Vector2.one).x);
    }

    public override void OnTouch()
    {
        if (!canRotate || !GameManager.Instance.tileHandler.canInteract) return;

        RotateReflect();
    }

    private void RotateReflect()
    {
        canRotate = false;

        if (transform.eulerAngles.z.Equals(0f))
        {
            transform.DORotate(new Vector3(0f, 0f, 90f), 0.2f)
                .OnComplete(() => canRotate = true);
        }
        else
        {
            transform.DORotate(Vector3.zero, 0.2f)
                .OnComplete(() => canRotate = true);
        }

        angleState *= -1;

        GameManager.Instance.OnRefresh();
    }

    public void ReflectSun(LineRenderer lr, Vector3 rayDir, int stack)
    {
        if (stack > 0)
            stack--;
        else
            return;

        lr.positionCount += 1;

        ChecRayDir(rayDir);

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, reflectDir, distance, whatIsTile);

        if (hit == null) return;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform != null && hit[i].transform != transform)
            {
                Tile tile = hit[i].transform.GetComponent<Tile>();

                if (tile.tileType.Equals(TileType.MIRROR))
                {
                    lr.SetPosition(lr.positionCount - 1, tile.transform.position);

                    MirrorTile mirrorTile = tile as MirrorTile;

                    //print(reflectDir);
                    mirrorTile.ReflectSun(lr, reflectDir, stack);

                    return;
                }

                if (tile.tileType.Equals(TileType.FIELD))
                {
                    lr.SetPosition(lr.positionCount - 1, tile.transform.position);

                    FieldTile fieldTile = tile as FieldTile;
                    fieldTile.GetSun();
                }
            }
        }

        lr.SetPosition(lr.positionCount - 1, transform.position + reflectDir * distance);
    }
    
    private void ChecRayDir(Vector3 rayDir)
    {
        if (rayDir.Equals(Vector3.right))
        {
            reflectDir = Vector3.down * angleState;
        }
        else if(rayDir.Equals(Vector3.left))
        {
            reflectDir = Vector3.up * angleState;
        }
        else if (rayDir.Equals(Vector3.up))
        {
            reflectDir = Vector3.left * angleState;
        }
        else if (rayDir.Equals(Vector3.down))
        {
            reflectDir = Vector3.right * angleState;
        }
    }
}
