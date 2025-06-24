using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private MoveDirection _reservedDirection = MoveDirection.None;
    public MoveDirection ReservedDirection => _reservedDirection;

    private float _inputX = 0.0f;
    private float _inputZ = 0.0f;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        else
            Destroy(this.gameObject);
    }

    public void ClearReservedDirection() => _reservedDirection = MoveDirection.None;

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputZ = Input.GetAxisRaw("Vertical");

        if (_reservedDirection == MoveDirection.None)
            SetDirection();
    }

    private void SetDirection()
    {
        if (_inputX != 0 && _inputZ != 0)
        {
            _reservedDirection = MoveDirection.None;
            return;
        }

        if (_inputX < 0) _reservedDirection = MoveDirection.Left;
        else if (_inputX > 0) _reservedDirection = MoveDirection.Right;
        else if (_inputZ > 0) _reservedDirection = MoveDirection.Up;
        else if (_inputZ < 0) _reservedDirection = MoveDirection.Down;
    }
}
