using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPositionFinder : MonoBehaviour
{
    [Header("����� ����� ���� Ű �ڵ�")]
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
