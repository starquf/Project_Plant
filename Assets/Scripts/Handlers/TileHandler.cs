using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileHandler : MonoBehaviour
{
    private Tile selectedTile;

    private LayerMask whatIsTile;
    private Vector3 target;

    private bool canInteract = true;

    private void Start()
    {
        GameManager.Instance.tileHandler = this;

        whatIsTile = LayerMask.GetMask("TILE");

        GameManager.Instance.onGameClear += () => canInteract = false;
    }

    private void Update()
    {
        if (!canInteract) return;

        GetTouch();
    }

    protected virtual void GetTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0f;

            Tile tile = Physics2D.OverlapPoint(target, whatIsTile)?.GetComponent<Tile>();

            // 클릭한 지점에 타일이 있다면
            if (tile != null && tile != selectedTile && tile.isClickable)
            {
                tile.OnTouch();
            }
            else if(selectedTile != null)
            {
                selectedTile.Select(false);
                selectedTile = null;
            }
        }
    }

    public void ChangeTile(Tile tile)
    {
        if (selectedTile == null)
        {
            selectedTile = tile;
            tile.Select(true);

            return;
        }

        Vector3 target = tile.transform.position;

        tile.transform.DOMove(selectedTile.transform.position, 0.2f);
        selectedTile.transform.DOMove(target, 0.2f)
            .OnComplete(() => GameManager.Instance.OnRefresh());

        selectedTile.Select(false);
        tile.Select(false);

        if (tile.transform.position == selectedTile.transform.position)
        {
            print("바뀜ㄴ");
        }

        selectedTile = null;
    }


}
