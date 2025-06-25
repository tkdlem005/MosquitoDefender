using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public static PlayerCharacter Instance;

    private PlayerController _controller;
    private Disinfection _disinfection;

    [SerializeField] private float _moveSpeed = 8.0f;
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private float _maxGasAmount = 50f;

    private Vector3? _moveTarget = null;

    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;

    public float MaxGasAmount => _maxGasAmount;

    protected override void Awake()
    {
        base.Awake();

        if (!Instance) Instance = this;
        else Destroy(this.gameObject);
    }

    protected override void Start()
    {
        base.Start();

        TryGetComponent<PlayerController>(out _controller);
        TryGetComponent<Disinfection>(out _disinfection);

        _disinfection.Execute();
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
        if (_moveTarget.HasValue)
        {
            Vector3 direction = (_moveTarget.Value - transform.position);
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }

    public void MoveTo(Vector3 target)
    {
        _moveTarget = target;
    }
}
