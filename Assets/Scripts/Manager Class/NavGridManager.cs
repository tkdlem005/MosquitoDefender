#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGridManager : MonoBehaviour
{
    public static NavGridManager Instance { get ; private set; }

    #region Container
    [SerializeField] private List<Vector2Int> _nonWalkableCells = new List<Vector2Int>();

    private Dictionary<Vector2Int, GridCell> _grid = new();
    #endregion

    [SerializeField] private int _gridWidth = 20;
    [SerializeField] private int _gridHeight = 20;
    [SerializeField] private float _cellSize = 1f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = -_gridWidth / 2; x < _gridWidth / 2; x++)
        {
            for(int y = -_gridHeight / 2; y < _gridHeight / 2; y++)
            {
                Vector2Int pos = new(x, y);
                _grid[pos] = new GridCell(pos, true);
            }
        }

        SetNonWalkableCells();
        SetBoundaryCellsNonWalkable();
    }

    #region Setter
    private void SetNonWalkableCells()
    {
        foreach (var pos in _nonWalkableCells) 
        {
            if (_grid.ContainsKey(pos))
                _grid[pos]._bIsWalkable = false;
            else
                Debug.LogWarning($"Non-Walkable Cell {pos} is outside of grid bounds");
        }
    }

    private void SetBoundaryCellsNonWalkable()
    {
        foreach (var pos in _grid.Keys)
        {
            if (pos.x == -_gridWidth / 2 || pos.x == _gridWidth / 2 - 1 || pos.y == -_gridHeight / 2 || pos.y == _gridHeight / 2 - 1)
            {
                _grid[pos]._bIsWalkable = false;
            }
        }
    }
    #endregion

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * _cellSize, PlayerCharacter.Instance.transform.position.y, gridPos.y * _cellSize);
    }

    public Vector2Int GetGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / _cellSize);
        int y = Mathf.RoundToInt(worldPos.z / _cellSize);

        return new Vector2Int(x, y);
    }

    public bool IsWalkable(Vector2Int gridPos) => _grid.ContainsKey(gridPos) && _grid[gridPos]._bIsWalkable;

    public Dictionary<Vector2Int, GridCell> GetGrid() => _grid;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_grid == null) return;

        foreach(var cell in _grid.Values)
        {
            Gizmos.color = cell._bIsWalkable ? Color.green : Color.red;
            Gizmos.DrawWireCube(GetWorldPosition(cell._gridPos), Vector3.one * _cellSize * 0.9f);


            Vector3 labelPos = GetWorldPosition(cell._gridPos);
            labelPos.y += 0.1f;
            Handles.Label(labelPos, $"{cell._gridPos.x},{cell._gridPos.y}");

        }
    }
#endif
}
