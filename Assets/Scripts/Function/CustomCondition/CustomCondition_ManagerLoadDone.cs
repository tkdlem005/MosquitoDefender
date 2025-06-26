using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCondition_ManagerLoadDone : CustomCondition
{
    private bool _checker;

    private void Awake() => EventManager.Instance.AddListener(EventList.EManagerAwake, OnEvent);

    public override void CompleteCondition(Action action) => 
        CoroutineDelegator.Instance.ExecuteCoroutine(ConditionWaiter(action));

    public void OnEvent(object param)
    {
        if (param == null)
            return;
        else
        {
            if(param is bool complete)
            {
                if(complete)
                    _checker = true;
            }

        }
    }

    private IEnumerator ConditionWaiter(Action action)
    {
        yield return new WaitUntil(() => _checker);
        _checker = false;

        action?.Invoke();
    }
}
