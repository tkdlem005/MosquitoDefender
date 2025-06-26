using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CustomCondition_PlayerDetector : CustomCondition
{
    private BoxCollider _boxCollider;

    private bool _checker = false;

    private void Awake()
    {
        TryGetComponent(out _boxCollider);
        _boxCollider.isTrigger = true;
    }

    public override void CompleteCondition(Action action)
        => CoroutineDelegator.Instance.ExecuteCoroutine(ConditionWaiter(action));

    private IEnumerator ConditionWaiter(Action action)
    {
        yield return new WaitUntil(() => _checker);

        _checker = false;
        action?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _checker = true;
    }
}
