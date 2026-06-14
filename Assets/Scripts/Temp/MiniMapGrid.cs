using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapGrid : MonoBehaviour
{
    public static MiniMapGrid Instance { get; private set; }

    [SerializeField] private RectTransform _mapContainer;
    [SerializeField] private GameObject _cellPrefab;

    private const float _cellSize = 20f; // 셀 크기 고정

    private int _gridWidth;
    private int _gridHeight;

    private Dictionary<Vector2Int, GameObject> _cellUIObjects = new();

    private Vector2Int _prevPlayerPos = new Vector2Int(int.MinValue, int.MinValue);
    [SerializeField] private Color _playerColor = new Color(1f, 0.5f, 0f, 1f); // 주황색

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); }

        EventManager.Instance.AddListener(EventList.EMapSettingDone, GenerateMiniMap);
    }

    public void GenerateMiniMap(object param)
    {
        var grid = WorldGridManager.Instance.GetGrid();
        _gridWidth = WorldGridManager.Instance.GridWidth;
        _gridHeight = WorldGridManager.Instance.GridHeight;

        // 패널 위치 및 크기 설정 (앵커 기준: Right Bottom)
        _mapContainer.anchorMin = new Vector2(1f, 0f);
        _mapContainer.anchorMax = new Vector2(1f, 0f);
        _mapContainer.pivot = new Vector2(1f, 0f);
        _mapContainer.anchoredPosition = new Vector2(320f, 50f);

        float panelWidth = _gridWidth * _cellSize;
        float panelHeight = _gridHeight * _cellSize;
        _mapContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth);
        _mapContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);

        // 셀 생성 및 배치
        foreach (var kvp in grid)
        {
            Vector2Int pos = kvp.Key;
            GridCell cell = kvp.Value;

            GameObject cellGO = Instantiate(_cellPrefab, _mapContainer);
            RectTransform rt = cellGO.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(_cellSize, _cellSize);

            // Right-Bottom 기준 위치 배치
            rt.anchoredPosition = new Vector2(
                -(_gridWidth - 1 - pos.x) * _cellSize,
                pos.y * _cellSize
            );

            UpdateCellColor(cellGO, cell);
            _cellUIObjects[pos] = cellGO;
        }
    }

    public void UpdateCellColor(GameObject cellGO, GridCell cell)
    {
        var img = cellGO.GetComponent<Image>();

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
            if (WorldGridManager.Instance.TryGetCell(pos, out var cell))
            {
                UpdateCellColor(cellGO, cell);
            }
        }
    }

    public void UpdatePlayerPosition(Vector2Int newPos)
    {
        // 이전 위치가 유효하면 원래 셀 색상으로 복원
        if (_cellUIObjects.TryGetValue(_prevPlayerPos, out var prevCellGO))
        {
            if (WorldGridManager.Instance.TryGetCell(_prevPlayerPos, out var prevCell))
                UpdateCellColor(prevCellGO, prevCell);
        }

        // 새로운 위치 셀 색상을 주황색으로 변경
        if (_cellUIObjects.TryGetValue(newPos, out var newCellGO))
        {
            var img = newCellGO.GetComponent<Image>();
            img.color = _playerColor;
        }

        _prevPlayerPos = newPos;
    }
}
