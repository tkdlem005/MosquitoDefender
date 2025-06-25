using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CustomCondition_PlayerDetector : CustomCondition
{
    private BoxCollider _boxCollider;

    public string _id;
    private bool _checker = false;
    private string _hiddenID = "CustomCondition_ListenEvent";

    private void Awake()
    {
        TryGetComponent(out _boxCollider);
        _boxCollider.isTrigger = true;
    }

    public override void CompleteCondition(Action action)
        => CoroutineDelegator.Instance.ExecuteCoroutine(_id + _hiddenID, ConditionWaiter(action));

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
