using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _owner;

    private Vector2Int _currentXZ;
    private Vector3 _targetWorldPos;
    private bool _isMoving = false;

    private void Awake() => EventManager.Instance.AddListener(EventList.EMapSettingDone, SetupController);

    private void Start() => _owner = PlayerCharacter.Instance;

    private void Update()
    {
        if (_isMoving) return;

        MoveDirection dir = InputManager.Instance.ReservedDirection;
        if (dir == MoveDirection.None) return;

        Vector2Int offsetXZ = DirectionToXZOffset(dir);
        Vector2Int nextXZ = _currentXZ + offsetXZ;

        if (NavGridManager.Instance.TryGetCell(nextXZ, out var nextCell) &&
            nextCell._bIsWalkable)
        {
            _currentXZ = nextCell._gridPosXZ;

            _targetWorldPos = NavGridManager.Instance.GetWorldPosition(_currentXZ);

            _owner.MoveTo(_targetWorldPos);
            _isMoving = true;
        }
        else
        {
            OnArrived();
        }
    }

    public void OnArrived()
    {
        InputManager.Instance.ClearReservedDirection();
        _isMoving = false;
    }

    private void SetupController(object param)
    {
        _currentXZ = NavGridManager.Instance.GetXZFromWorld(transform.position);
        _targetWorldPos = transform.position;
        EventManager.Instance.TriggerEvent(EventList.EControllerSettingDone);
    }

    private Vector2Int DirectionToXZOffset(MoveDirection dir)
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

