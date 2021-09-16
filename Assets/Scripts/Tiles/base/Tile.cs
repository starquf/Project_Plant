using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public TileType tileType;

    // 상호작용 가능한 것인지
    public bool isClickable = true;

    private Vector3 target = Vector3.zero;
    private float sizeX = 0.5f;

    protected virtual void Start()
    {
        sizeX = transform.localScale.x / 2f;
    }

    protected virtual void Update()
    {
        GetTouch();
    }

    protected virtual void GetTouch()
    {
        if (isClickable && Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0f;

            Vector3 dir = target - transform.position;

            // 클릭한 지점이 범위 안에 있다면
            if (dir.sqrMagnitude <= (sizeX * sizeX))
            {
                OnTouch();
            }
        }
    }

    protected virtual void OnTouch()
    {
        // Do something
    }
}
