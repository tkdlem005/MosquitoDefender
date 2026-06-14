using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Childrens : Character, IFreezable
{
    private enum AIState
    {
        Patrolling,
        Chasing
    }

    private Transform _targetPlayer;
    private PathFinding _pathFinding;

    private List<Vector2Int> _currentPath = new();
    private int _pathIndex = 0;
    private float _pathTimer = 0f;

    private float _lastY;
    private float _lastPitchAngle = 0f;
    private bool _isFrozen = false;

    [SerializeField] private float _remainTime = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _pathUpdateInterval = 0.5f;

    [SerializeField] private float _playerStunTime = 3f;

    public bool IsFrozen => _isFrozen;

    protected void OnEnable() => StartCoroutine(DestroyAfterTime(_remainTime));

    protected override void Start()
    {
        base.Start();

        _pathFinding = new PathFinding(WorldGridManager.Instance);
    }


    protected override void Update()
    {
        base.Update();

        if (_isFrozen) return;

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
        else
        {
            DetectPlayer();
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
            Vector3 target = WorldGridManager.Instance.GetWorldPosition(_currentPath[_pathIndex]);
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
        Vector2Int start = WorldGridManager.Instance.GetXZFromWorld(transform.position);
        Vector2Int end = WorldGridManager.Instance.GetXZFromWorld(_targetPlayer.position);

        _currentPath = _pathFinding.FindPath(start, end);
        _pathIndex = 0;
    }

    private void MoveAlongPath()
    {
        if (_currentPath == null || _currentPath.Count == 0 || _pathIndex >= _currentPath.Count)
            return;

        Vector3 targetPos = WorldGridManager.Instance.GetWorldPosition(_currentPath[_pathIndex]);

        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * _moveSpeed * Time.deltaTime;

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < 0.1f)
        {
            _pathIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        if (_currentPath == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < _currentPath.Count - 1; i++)
        {
            Vector3 from = WorldGridManager.Instance.GetWorldPosition(_currentPath[i]);
            Vector3 to = WorldGridManager.Instance.GetWorldPosition(_currentPath[i + 1]);
            Gizmos.DrawLine(from, to);
        }

        Vector3 startPos = WorldGridManager.Instance.GetWorldPosition(_currentPath[0]);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, 0.3f);

        Vector3 endPos = WorldGridManager.Instance.GetWorldPosition(_currentPath[_currentPath.Count - 1]);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPos, 0.3f);
    }

    #region IFreezable
    public void Freeze(float duration)
    {
        if (!IsFrozen)
        {
            StartCoroutine(ChildrensFreezeRoutine(duration));
        }
    }

    private IEnumerator ChildrensFreezeRoutine(float duration)
    {
        _isFrozen = true;
        _targetPlayer = null;
        _characterCollider.enabled = false;

        yield return new WaitForSeconds(duration);

        _characterCollider.enabled = true;
        _isFrozen = false;
    }
    #endregion

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player 충돌 감지시 Player Freeze 적용
        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent(out IFreezable freezable))
            {
                if (freezable.IsFrozen)
                {
                    Destroy(gameObject);
                    return;
                }

                _characterCollider.enabled = false;
                freezable.Freeze(_playerStunTime);
                _isFrozen = true;

                EventManager.Instance.TriggerEvent(EventList.EOnCrashEvent);
            }
        }
    }
}