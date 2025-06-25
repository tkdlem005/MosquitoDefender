using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridVisualDeberger : MonoBehaviour
{
    [SerializeField] private NavGridData _gridData;
    [SerializeField] private bool _bIsSeeLabel = true;

    private Dictionary<Vector2Int, bool> _visualGrid = new();

    private void OnValidate()
    {
        if (_gridData == null)
            return;

        GenerateVisualGrid();
    }

    private void GenerateVisualGrid()
    {
        _visualGrid.Clear();

        int width = _gridData.GridWidth;
        int height = _gridData.GridHeight;

        int halfWidth = width / 2;
        int halfHeight = height / 2;

        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int z = -halfHeight; z < halfHeight; z++)
            {
                Vector2Int pos = new(x, z);

                bool isEdge = (x == -halfWidth || x == halfWidth - 1 || z == -halfHeight || z == halfHeight - 1);
                bool isNonWalkable = _gridData.CustomNonWalkables != null && _gridData.CustomNonWalkables.Contains(pos);

                _visualGrid[pos] = isEdge || isNonWalkable;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_gridData == null)
            return;

        GenerateVisualGrid();

        foreach (var kvp in _visualGrid)
        {
            Vector2Int pos = kvp.Key;
            bool isRed = kvp.Value;

            float y = _gridData.DefaultY;
            foreach (var custom in _gridData.CustomHeights)
            {
                if (custom.XZ == pos)
                {
                    y = custom.Y;
                    break;
                }
            }

            Gizmos.color = isRed ? Color.red : Color.green;
            Vector3 worldPos = new Vector3(pos.x * _gridData.CellSize, y * _gridData.CellSize, pos.y * _gridData.CellSize);
            Gizmos.DrawWireCube(worldPos, Vector3.one * _gridData.CellSize * 0.9f);

            if (_bIsSeeLabel)
                Handles.Label(worldPos + Vector3.up * 0.1f, $"{pos.x},{y},{pos.y}");
        }
    }
#endif
}
