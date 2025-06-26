using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomCondition_ListenEvent : CustomCondition
{
    public EventList _eventList;

    private bool _checker = false;

    private void Awake() => EventManager.Instance.AddListener(_eventList, OnEvent);

    private void OnDestroy() => EventManager.Instance.RemoveListener(_eventList, OnEvent);

    public override void CompleteCondition(Action action) => 
        CoroutineDelegator.Instance.ExecuteCoroutine(ConditionWaiter(action));

    private void OnEvent(object param) => _checker = true;

    IEnumerator ConditionWaiter(Action action)
    {
        yield return new WaitUntil(() => _checker);
        _checker = false;

        action?.Invoke();
    }
}
