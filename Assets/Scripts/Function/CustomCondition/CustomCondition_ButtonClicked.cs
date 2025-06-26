using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomCondition_ButtonClicked : CustomCondition
{
    [SerializeField] private bool _checker = false;

    public override void CompleteCondition(Action action) => 
        CoroutineDelegator.Instance.ExecuteCoroutine(ConditionWaiter(action));

    public void OnEvent()
    {
        _checker = true;
    }

    private IEnumerator ConditionWaiter(Action action)
    {
        yield return new WaitUntil(() => _checker);
        _checker = false;

        action?.Invoke();
    }
}
