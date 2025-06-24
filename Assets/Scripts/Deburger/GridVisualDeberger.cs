using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridVisualDeberger : MonoBehaviour
{
    public float _cellSize = 1f;
    public int _gridWidth = 10;
    public int _gridHeight = 10;

    // »¡°£»öÀ¸·Î Ç¥½ÃÇÒ ³»ºÎ ÁÂÇ¥ ¸ñ·Ï
    public List<Vector2Int> RedCells = new();

    private Dictionary<Vector2Int, bool> _testGrid = new();

    private void OnValidate()
    {
        GenerateTestGrid();
    }

    private void GenerateTestGrid()
    {
        _testGrid.Clear();

        int halfWidth = _gridWidth / 2;
        int halfHeight = _gridHeight / 2;

        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = -halfHeight; y < halfHeight; y++)
            {
                Vector2Int pos = new(x, y);

                // ¿Ü°û Á¶°Ç
                bool isEdge = (x == -halfWidth || x == halfWidth - 1 || y == -halfHeight || y == halfHeight - 1);
                bool isMarked = RedCells.Contains(pos);

                _testGrid[pos] = isEdge || isMarked;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_testGrid == null || _testGrid.Count == 0)
            GenerateTestGrid();

        foreach (var kvp in _testGrid)
        {
            var pos = kvp.Key;
            bool isRed = kvp.Value;

            Gizmos.color = isRed ? Color.red : Color.green;
            Gizmos.DrawWireCube(GetWorldPosition(pos), Vector3.one * _cellSize * 0.9f);

            Vector3 labelPos = GetWorldPosition(pos);
            labelPos.y += 0.1f;
            Handles.Label(labelPos, $"{pos.x},{pos.y}");
        }
    }
#endif

    private Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * _cellSize, 0, gridPos.y * _cellSize);
    }
}
