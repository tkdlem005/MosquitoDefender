using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _owner;

    private Vector2Int _currentGridPos;
    private Vector3 _targetWorldPos;
    private bool _isMoving = false;

    private void Start()
    {
        _owner = PlayerCharacter.Instance;
        _currentGridPos = NavGridManager.Instance.GetGridPosition(transform.position);
        _targetWorldPos = transform.position;
    }

    private void Update()
    {
        if (_isMoving) return;

        MoveDirection dir = InputManager.Instance.ReservedDirection;
        if (dir == MoveDirection.None) return;

        Vector2Int offset = DirectionToVector(dir);
        Vector2Int nextGrid = _currentGridPos + offset;

        if (NavGridManager.Instance.IsWalkable(nextGrid))
        {
            _currentGridPos = nextGrid;
            _targetWorldPos = NavGridManager.Instance.GetWorldPosition(nextGrid);
            _owner.MoveTo(_targetWorldPos);
            _isMoving = true;
        }
        else
            OnArrived();
    }

    public void OnArrived()
    {
        InputManager.Instance.ClearReservedDirection();
        _isMoving = false;
    }

    private Vector2Int DirectionToVector(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => new Vector2Int(0, 1),
            MoveDirection.Down => new Vector2Int(0, -1),
            MoveDirection.Left => new Vector2Int(-1, 0),
            MoveDirection.Right => new Vector2Int(1, 0),
            _ => Vector2Int.zero
        };
    }
}
