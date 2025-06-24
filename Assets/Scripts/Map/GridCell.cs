using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector2Int _gridPosXZ;
    public float _hegihtY = 1.1f;
    public bool _bIsWalkable = true;
    public bool _bIsClean = false;

    public GridCell(Vector2Int gridPosXZ, float heightY, bool bIsWalkable)
    {
        _gridPosXZ = gridPosXZ;
        _hegihtY = heightY;
        _bIsWalkable = bIsWalkable;
    }

    public Vector3 GetGridPos()
    {
        return new Vector3(_gridPosXZ.x, _hegihtY, _gridPosXZ.y);
    }
}
