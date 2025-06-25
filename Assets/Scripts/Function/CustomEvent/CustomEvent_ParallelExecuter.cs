using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomEvent_ParallelExecuter : CustomEvent
{
    [Header("이벤트들을 동시에 실행하고 싶을 때 사용합니다.")]
    [Space(10f)]

    [Header("동시에 실행시킬 이벤트들을 끌어다 넣어주세요.")]
    [Tooltip("동시에 실행할 CustomEvent 리스트")]
    [SerializeField] private List<CustomEvent> _subEvents;
    [SerializeField] private string _id;

    private string _hiddenID = "Event_ParallelExecuter";

    public override void ExecuteEvent(Action action)
    {
        if (_subEvents == null || _subEvents.Count == 0)
        {
            Debug.LogWarning("ParallelCustomEvent: 실행할 서브 이벤트가 없습니다.");
            action?.Invoke();
            return;
        }

        CoroutineDelegator.Instance.ExecuteCoroutine(_hiddenID + _id, RunParallelEvents(action));
    }

    private IEnumerator RunParallelEvents(Action action)
    {
        int completedCount = 0;
        Action onSubEventComplete = () => completedCount++;

        for (int i = 0; i < _subEvents.Count; i++)
        {
            if (_subEvents[i] == null) { continue; }

            _subEvents[i].ExecuteEvent(onSubEventComplete);
        }

        yield return new WaitUntil(() => completedCount >= _subEvents.Count);

        Debug.Log("모든 서브 이벤트 완료.");
        action?.Invoke();
    }
}
