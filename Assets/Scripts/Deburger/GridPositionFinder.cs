using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositionFinder : MonoBehaviour
{
    [Header("디버그 사용을 위한 키 코드")]
    public KeyCode _keyCode;

    public Vector2Int _cellPos;

    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            Vector3 worldPosition = NavGridManager.Instance.GetWorldPosition(_cellPos);
            Debug.Log($"[{_cellPos}] Center Position : {worldPosition.ToString()}");
        }
    }
}
