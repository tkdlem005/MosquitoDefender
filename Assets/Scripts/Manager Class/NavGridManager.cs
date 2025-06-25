using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavGridManager : ManagerBase
{
    public static NavGridManager Instance { get; private set; }

    [SerializeField] private NavGridData _gridData;

    private Dictionary<Vector2Int, GridCell> _grid = new();
    private Dictionary<Vector2Int, bool> _cleanStatus = new();

    [SerializeField] private bool _useDebuger = true;

    private void Awake() => Initialize();

    private void OnDestroy() => EventManager.Instance.RemoveListener(EventList.ESettingMap, InitializeGrid);

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ESettingMap, InitializeGrid);

        InitializeEnd();
    }
    private void InitializeGrid(object param = null)
    {
        if (param is StageData stageData)
        {
            _gridData = stageData._mapData;
        }

        _grid.Clear();
        _cleanStatus.Clear();

        int width = _gridData.GridWidth;
        int height = _gridData.GridHeight;
        float defaultY = _gridData.DefaultY;
        float cellSize = _gridData.CellSize;

        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int z = -height / 2; z < height / 2; z++)
            {
                Vector2Int posXZ = new(x, z);
                float heightY = GetCustomHeight(posXZ, defaultY);
                bool isWalkable = true;

                _grid[posXZ] = new GridCell(posXZ, heightY, isWalkable);
            }
        }

        SetCustomNonWalkables();
        SetBoundaryCellsNonWalkable();

        foreach (var kvp in _grid)
        {
            if (kvp.Value._bIsWalkable)
            {
                _cleanStatus[kvp.Key] = kvp.Value._bIsClean;
            }
        }
    }
    private void SetCustomNonWalkables()
    {
        if (_gridData.CustomNonWalkables == null)
            return;

        foreach (var pos in _gridData.CustomNonWalkables)
        {
            if (_grid.TryGetValue(pos, out var cell))
            {
                cell._bIsWalkable = false;
            }
        }
    }

    private void SetBoundaryCellsNonWalkable()
    {
        int width = _gridData.GridWidth;
        int height = _gridData.GridHeight;

        foreach (var kvp in _grid)
        {
            Vector2Int pos = kvp.Key;
            if (pos.x == -width / 2 || pos.x == width / 2 - 1 ||
                pos.y == -height / 2 || pos.y == height / 2 - 1)
            {
                kvp.Value._bIsWalkable = false;
            }
        }
    }

    public void SetCleanStatus(Vector2Int pos, bool isClean)
    {
        if (_cleanStatus.ContainsKey(pos))
        {
            _cleanStatus[pos] = isClean;
            if (_grid.TryGetValue(pos, out var cell))
            {
                cell._bIsClean = isClean;
            }
        }
    }
    private float GetCustomHeight(Vector2Int pos, float defaultY)
    {
        foreach (var custom in _gridData.CustomHeights)
        {
            if (custom.XZ == pos)
                return custom.Y;
        }
        return defaultY;
    }

    public bool TryGetCleanStatus(Vector2Int pos, out bool isClean)
    {
        return _cleanStatus.TryGetValue(pos, out isClean);
    }


    public Vector3 GetWorldPosition(Vector2Int xz)
    {
        if (_grid.TryGetValue(xz, out var cell))
            return new Vector3(xz.x * _gridData.CellSize, cell._hegihtY * _gridData.CellSize, xz.y * _gridData.CellSize);
        return Vector3.zero;
    }

    public Vector2Int GetXZFromWorld(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / _gridData.CellSize);
        int z = Mathf.RoundToInt(worldPos.z / _gridData.CellSize);
        return new Vector2Int(x, z);
    }

    public bool TryGetCell(Vector2Int pos, out GridCell cell)
    {
        return _grid.TryGetValue(pos, out cell);
    }

    public Dictionary<Vector2Int, bool> GetCleanStatusDictionary() => _cleanStatus;

    public bool IsWalkable(Vector2Int xz) => _grid.TryGetValue(xz, out var cell) && cell._bIsWalkable;

    public Dictionary<Vector2Int, GridCell> GetGrid() => _grid;

    private void OnDrawGizmos()
    {
        if (_useDebuger)
        {
            if (_grid == null || _grid.Count == 0) return;

            foreach (var cell in _grid.Values)
            {
                if (cell._bIsClean)
                    Gizmos.color = Color.yellow;
                else
                    Gizmos.color = cell._bIsWalkable ? Color.green : Color.red;

                Vector3 pos = new(
                    cell._gridPosXZ.x * _gridData.CellSize,
                    cell._hegihtY * _gridData.CellSize,
                    cell._gridPosXZ.y * _gridData.CellSize
                );

                Gizmos.DrawWireCube(pos, Vector3.one * _gridData.CellSize * 0.9f);
            }
        }
    }

}