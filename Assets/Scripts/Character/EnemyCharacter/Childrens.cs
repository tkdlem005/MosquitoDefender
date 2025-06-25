using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Childrens : Character
{
    private enum AIState
    {
        Patrolling,
        Chasing
    }

    private Transform _targetPlayer;
    private Pathfinding _pathFinding;

    private List<Vector2Int> _currentPath = new();
    private int _pathIndex = 0;
    private float _pathTimer = 0f;

    private float _lastY;
    private float _lastPitchAngle = 0f;

    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _pathUpdateInterval = 0.5f;

    protected override void Start()
    {
        base.Start();

        _pathFinding = new Pathfinding(NavGridManager.Instance);
    }


    protected override void Update()
    {
        base.Update();

        DetectPlayer();

        if (_targetPlayer != null)
        {
            _pathTimer += Time.deltaTime;
            if (_pathTimer >= _pathUpdateInterval)
            {
                _pathTimer = 0f;
                UpdatePath();
            }

            MoveAlongPath();
        }
    }

    private void LateUpdate()
    {
        float currentY = transform.position.y;
        float pitchAngle = 0f;
        bool forceSnapRotation = false;
        bool isRampArea = false;

        float verticalDiff = 0f;

        if (_pathIndex < _currentPath.Count)
        {
            Vector3 target = NavGridManager.Instance.GetWorldPosition(_currentPath[_pathIndex]);
            verticalDiff = target.y - currentY;
        }
        else
        {
            verticalDiff = currentY - _lastY;
        }

        if (Mathf.Abs(currentY - 0.6f) < 0.05f || Mathf.Abs(currentY - 1.6f) < 0.05f)
        {
            isRampArea = true;

            if (_pathIndex < _currentPath.Count)
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
        else if (_pathIndex < _currentPath.Count && Mathf.Abs(verticalDiff) > 0.05f)
        {
            pitchAngle = verticalDiff < 0 ? 15f : -15f;
            _lastPitchAngle = pitchAngle;
        }

        float fixedYaw = 0f;

        Quaternion targetRotation = Quaternion.Euler(pitchAngle, fixedYaw, 0f);

        if (forceSnapRotation)
        {
            transform.rotation = targetRotation;
        }
        else if (_pathIndex < _currentPath.Count)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        else
        {
            Quaternion flatRotation = Quaternion.Euler(0f, fixedYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, flatRotation, Time.deltaTime * _rotationSpeed);

            if (!isRampArea && Quaternion.Angle(transform.rotation, flatRotation) < 0.1f)
            {
                _lastPitchAngle = 0f;
            }
        }

        _lastY = currentY;
    }




    private void DetectPlayer()
    {
        if (_targetPlayer == null)
        {
            GameObject playerObj = PlayerCharacter.Instance.gameObject;
            if (playerObj != null)
                _targetPlayer = playerObj.transform;
        }

        if (_targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, _targetPlayer.position);
            if (distance > _chaseRange)
            {
                _targetPlayer = null;
                _currentPath.Clear();
                _pathTimer = 0;
            }
        }
    }

    private void UpdatePath()
    {
        Vector2Int start = NavGridManager.Instance.GetXZFromWorld(transform.position);
        Vector2Int end = NavGridManager.Instance.GetXZFromWorld(_targetPlayer.position);

        _currentPath = _pathFinding.FindPath(start, end);
        _pathIndex = 0;
    }

    private void MoveAlongPath()
    {
        if (_currentPath == null || _currentPath.Count == 0 || _pathIndex >= _currentPath.Count)
            return;

        Vector3 targetPos = NavGridManager.Instance.GetWorldPosition(_currentPath[_pathIndex]);

        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * _moveSpeed * Time.deltaTime;

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < 0.1f)
        {
            _pathIndex++;
        }
    }
}
