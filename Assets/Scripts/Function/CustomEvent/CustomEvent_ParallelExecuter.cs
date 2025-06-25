using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomEvent_ParallelExecuter : CustomEvent
{
    [Header("�̺�Ʈ���� ���ÿ� �����ϰ� ���� �� ����մϴ�.")]
    [Space(10f)]

    [Header("���ÿ� �����ų �̺�Ʈ���� ����� �־��ּ���.")]
    [Tooltip("���ÿ� ������ CustomEvent ����Ʈ")]
    [SerializeField] private List<CustomEvent> _subEvents;
    [SerializeField] private string _id;

    private string _hiddenID = "Event_ParallelExecuter";

    public override void ExecuteEvent(Action action)
    {
        if (_subEvents == null || _subEvents.Count == 0)
        {
            Debug.LogWarning("ParallelCustomEvent: ������ ���� �̺�Ʈ�� �����ϴ�.");
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

        Debug.Log("��� ���� �̺�Ʈ �Ϸ�.");
        action?.Invoke();
    }
}
