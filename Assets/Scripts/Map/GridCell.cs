using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector2Int _gridPos;
    public bool _bIsWalkable = true;

    public GridCell(Vector2Int gridPos, bool bIsWalkable)
    {
        _gridPos = gridPos;
        _bIsWalkable = bIsWalkable;
    }
}
