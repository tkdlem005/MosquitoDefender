using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : Character
{
    public static PlayerCharacter Instance {  get; private set; }

    [SerializeField] private PlayerController _controller;
    [SerializeField] private Disinfection _disinfection;
    [SerializeField] private Horn _horn;

    [SerializeField] private float _moveSpeed = 8.0f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _maxGasAmount = 50f;
    [SerializeField] private float _hornRadius = 1.5f;

    private Vector3? _moveTarget = null;
    private float _lastY;
    private float _lastPitchAngle = 0f;
    private bool _isFreeze;

    public Horn Horn => _horn;

    public float HornRadius => _hornRadius;
    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float MaxGasAmount => _maxGasAmount;

    public bool IsFreeze => _isFreeze;

    protected override void Awake()
    {
        base.Awake();

        if (!Instance) Instance = this;
        else Destroy(this.gameObject);

        EventManager.Instance.AddListener(EventList.EGameStart, PlayerStart);
    }

    protected override void Update()
    {
        base.Update();

        if (_moveTarget.HasValue)
        {
            Vector3 target = _moveTarget.Value;
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                transform.position = target;
                _moveTarget = null;

                _controller.OnArrived();
            }
        }
    }

    private void LateUpdate()
    {
        float currentY = transform.position.y;
        float yaw = transform.rotation.eulerAngles.y;
        float pitchAngle = 0f;
        bool forceSnapRotation = false;
        bool isRampArea = false;

        float verticalDiff = 0f;

        if (_moveTarget.HasValue) verticalDiff = _moveTarget.Value.y - currentY;
        else verticalDiff = currentY - _lastY;

        if (_moveTarget.HasValue)
        {
            Vector3 direction = _moveTarget.Value - transform.position;
            Vector3 flatDirection = direction;
            flatDirection.y = 0f;

            if (flatDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetYawRotation = Quaternion.LookRotation(flatDirection.normalized);
                yaw = targetYawRotation.eulerAngles.y;
            }
        }

        if (Mathf.Abs(currentY - 0.6f) < 0.05f || Mathf.Abs(currentY - 1.6f) < 0.05f)
        {
            isRampArea = true;

            if (_moveTarget.HasValue)
            {
                pitchAngle = verticalDiff < 0 ? 15f : -15f;
                _lastPitchAngle = pitchAngle;
            }
            else
            {
                pitchAngle = _lastPitchAngle;
            }

            forceSnapRotation = true;
        }
        else if (_moveTarget.HasValue && Mathf.Abs(verticalDiff) > 0.05f)
        {
            pitchAngle = verticalDiff < 0 ? 15f : -15f;
            _lastPitchAngle = pitchAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(pitchAngle, yaw, 0f);

        if (forceSnapRotation)
        {
            transform.rotation = targetRotation;
        }
        else if (_moveTarget.HasValue)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        else
        {
            Quaternion flatRotation = Quaternion.Euler(0f, yaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, flatRotation, Time.deltaTime * _rotationSpeed);

            if (!isRampArea && Quaternion.Angle(transform.rotation, flatRotation) < 0.1f) _lastPitchAngle = 0f;
        }

        _lastY = currentY;
    }

    public void ResetPlayer()
    {
        _moveTarget = null;
        _isFreeze = false;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _moveSpeed = 8.0f;
        _hornRadius = 1.5f;
        _maxGasAmount = 50f;
    }

    private void PlayerStart(object param)
    {
        _disinfection.Execute();
    }

    public void Freeze(float time) => StartCoroutine(FreezePlayer(time));

    public void MoveTo(Vector3 target)
    {
        _moveTarget = target;
    }

    public void IncreaseStatus(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Speed:
                _moveSpeed += 2;
                break;

            case AbilityType.Gas:
                _maxGasAmount += 15;
                break;

            case AbilityType.Horn:
                _hornRadius += 1.5f;
                break;
        }
    }

    IEnumerator FreezePlayer(float time)
    {
        InputManager.Instance.CanMove = false;
        _isFreeze = true;

        yield return new WaitForSeconds(time);

        InputManager.Instance.CanMove = true;
        _isFreeze = false;
    }
}
