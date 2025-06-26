using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapGrid : MonoBehaviour
{
    [SerializeField] private RectTransform _mapContainer;
    [SerializeField] private GameObject _cellPrefab;

    private float _cellSize;
    private int _gridWidth;
    private int _gridHeight;

    private Dictionary<Vector2Int, GameObject> _cellUIObjects = new();

    public void Awake()
    {
        EventManager.Instance.AddListener(EventList.EMapSettingDone, GenerateMiniMap);
    }

    public void GenerateMiniMap(object param)
    {
        var grid = NavGridManager.Instance.GetGrid();

        _gridWidth = NavGridManager.Instance.GridWidth;
        _gridHeight = NavGridManager.Instance.GridHeight;

        _cellSize = 20f;

        float panelWidth = _gridWidth * _cellSize;
        float panelHeight = _gridHeight * _cellSize;

        _mapContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth);
        _mapContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);

        // ¼¿ »ý¼º
        foreach (var kvp in grid)
        {
            Vector2Int pos = kvp.Key;
            GridCell cell = kvp.Value;

            GameObject cellGO = Instantiate(_cellPrefab, _mapContainer);
            RectTransform rt = cellGO.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(_cellSize, _cellSize);

            // Áß¾Ó Á¤·Ä ¿ÀÇÁ¼Â (Â¦¼ö/È¦¼ö¿¡ µû¶ó Á¶Àý)
            float offsetX = (_gridWidth / 2f - (_gridWidth % 2 == 0 ? 0.5f : 0f)) * _cellSize;
            float offsetY = (_gridHeight / 2f - (_gridHeight % 2 == 0 ? 0.5f : 0f)) * _cellSize;

            rt.anchoredPosition = new Vector2(
                pos.x * _cellSize - offsetX,
                pos.y * _cellSize - offsetY
            );

            UpdateCellColor(cellGO, cell);

            _cellUIObjects[pos] = cellGO;
        }
    }


    public void UpdateCellColor(GameObject cellGO, GridCell cell)
    {
        var img = cellGO.GetComponent<UnityEngine.UI.Image>();

        if (!cell._bIsWalkable)
            img.color = Color.black;
        else if (!cell._bIsClean)
            img.color = Color.gray;
        else
            img.color = Color.green;
    }

    public void RefreshCell(Vector2Int pos)
    {
        if (_cellUIObjects.TryGetValue(pos, out var cellGO))
        {
            if (NavGridManager.Instance.TryGetCell(pos, out var cell))
            {
                UpdateCellColor(cellGO, cell);
            }
        }
    }
}
