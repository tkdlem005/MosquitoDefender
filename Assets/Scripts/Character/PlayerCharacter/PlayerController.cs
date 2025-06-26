using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _owner;
    [SerializeField] private Horn _horn;

    [SerializeField] private Vector2Int _currentXZ;
    [SerializeField] private Vector3 _targetWorldPos;
    private bool _isMoving = false;

    private void Awake()
    {
        EventManager.Instance.AddListener(EventList.EMapSettingDone, SetupController);
        EventManager.Instance.AddListener(EventList.EStageEnd, ResetController);
    }

    private void Start() => _owner = PlayerCharacter.Instance;

    private void Update()
    {
        if (_isMoving) return;

        Vector2 input = InputManager.Instance.RawInput;

        // 반드시 한 방향만 허용 (대각선 무시)
        if (Mathf.Abs(input.x) > 0 && Mathf.Abs(input.y) > 0)
        {
            // 둘 다 입력된 경우 → 무시하거나 한쪽만 사용 (예: x 우선)
            input.y = 0; // 또는 input.x = 0; ← 선택지
        }

        if (input == Vector2.zero)
            return;

        // 플레이어 로컬 기준 방향 (정방향만 입력됨)
        Vector3 localMoveDir = new Vector3(input.x, 0, input.y).normalized;

        // 월드 기준 이동 방향 (플레이어 회전 적용)
        Vector3 worldMoveDir = _owner.transform.rotation * localMoveDir;

        // 정수 방향으로 정리
        Vector2Int offsetXZ = new Vector2Int(
            Mathf.RoundToInt(worldMoveDir.x),
            Mathf.RoundToInt(worldMoveDir.z)
        );

        // 다음 위치 계산
        Vector2Int nextXZ = _currentXZ + offsetXZ;

        if (NavGridManager.Instance.TryGetCell(nextXZ, out var nextCell) && nextCell._bIsWalkable)
        {
            _currentXZ = nextCell._gridPosXZ;
            _targetWorldPos = NavGridManager.Instance.GetWorldPosition(_currentXZ);

            // 플레이어가 이동 방향을 바라보게 회전 (회전은 4방향만 허용)
            Vector3 lookDirection = new Vector3(offsetXZ.x, 0, offsetXZ.y);
            if (lookDirection != Vector3.zero)
                _owner.transform.rotation = Quaternion.LookRotation(lookDirection);

            _owner.MoveTo(_targetWorldPos);
            _isMoving = true;
        }
        else
        {
            OnArrived();
        }
    }



    public void ResetController(object param)
    {
        InputManager.Instance.ClearReservedDirection();

        _isMoving = false;

        if (_owner == null)
            _owner = PlayerCharacter.Instance;
    }

    public void OnArrived()
    {
        InputManager.Instance.ClearReservedDirection();
        _isMoving = false;
    }

    private void SetupController(object param)
    {
        Debug.Log("Receive Event");

        _currentXZ = NavGridManager.Instance.GetXZFromWorld(PlayerCharacter.Instance.transform.position);
        _targetWorldPos = PlayerCharacter.Instance.transform.position;

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

