using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public TileType tileType;

    // ��ȣ�ۿ� ������ ������
    public bool isClickable = true;

    public virtual void OnTouch()
    {
        // Do something
    }

    public virtual void Select(bool isSelect)
    {
        
    }
}
