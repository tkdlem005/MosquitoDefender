using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : ManagerBase
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private MoveDirection _reservedDirection = MoveDirection.None;
    public MoveDirection ReservedDirection => _reservedDirection;

    [SerializeField] private bool _canMove = false;

    private float _inputX = 0.0f;
    private float _inputZ = 0.0f;

    public bool CanMove { get { return _canMove; }  set { _canMove = value; } }

    public Vector2 RawInput => new Vector2(_inputX, _inputZ);

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance)
            Instance = this;

        else
            Destroy(this.gameObject);

        InitializeEnd();
    }

    protected override void ResetManager(object param)
    {
        _inputX = 0.0f;
        _inputZ = 0.0f;

        _reservedDirection = MoveDirection.None;

        _canMove = false;
    }

    public void ClearReservedDirection() => _reservedDirection = MoveDirection.None;

    private void Update()
    {
        if (CanMove)
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            { 
                if(PlayerCharacter.Instance != null)
                {
                    if (PlayerCharacter.Instance.IsChargeStation)
                    {
                        PlayerCharacter.Instance.Disinfection.SetGas(PlayerCharacter.Instance.MaxGasAmount);
                    }
                    else
                    {
                        PlayerCharacter.Instance.Horn.Activate();
                    }
                }
            }

            _inputX = Input.GetAxisRaw("Horizontal");
            _inputZ = Input.GetAxisRaw("Vertical");

            if (_reservedDirection == MoveDirection.None)
                SetDirection();
        }
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
