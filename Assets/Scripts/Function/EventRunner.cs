using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    public enum RunnerMode
    {
        �̼���,
        ����_��_����,
        ����_��_���,
        ����_��_��Ȱ��ȭ
    }

    public string _RunnerID = "";

    [Header("������ ���� ���")]
    [SerializeField] private RunnerMode _mode = RunnerMode.�̼���;

    [Space(20f), Header("���� ���� ����")]
    public List<CustomCondition> _customStartConditions;

    [Header("��� ������ �����ؾ� ���")]
    public bool _necessaryAndSufficientStartCondition = false;

    [Space(20f), Header("������ �̺�Ʈ ����"), Tooltip("������ ��� Event���� ������ �Ʒ��� ������� �����մϴ�.")]
    public List<CustomEvent> _customEvents;

    [Space(20f), Header("���� ���� ����")]
    public List<CustomCondition> _customEndConditions;

    [Header("��� ������ �����ؾ� ���")]
    public bool _necessaryAndSufficientEndCondition = false;

    private bool _endConditionSignal;
    public bool EndSignal {get; private set;}

    public void Start()
    {
        if (_customStartConditions != null && _customStartConditions.Count > 0)
        {
            CoroutineDelegator.Instance.ExecuteCoroutine(
                _RunnerID + "CheckCondition",
                CheckCondition(
                    _customStartConditions,
                    _necessaryAndSufficientStartCondition,
                    () => CoroutineDelegator.Instance.ExecuteCoroutine(_RunnerID + "Run", Run()))
                );
        }
        else CoroutineDelegator.Instance.ExecuteCoroutine(_RunnerID + "Run", Run());
    }

    private IEnumerator CheckCondition(List<CustomCondition> conditions, bool requireAll, Action onComplete = null)
    {
        bool isNull = false;
        bool[] conditionResult = new bool[conditions.Count];
        Action<int> onConditionsCompleted = index => conditionResult[index] = true;

        for(int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i] == null)
            {
                isNull = true;
                break;
            }

            int index = i;
            conditions[i].CompleteCondition(() => onConditionsCompleted(index));
        }

        if (!isNull)
        {
            if (requireAll)
                yield return new WaitUntil(() => Array.TrueForAll(conditionResult, result => result));
            else
                yield return new WaitUntil(() => Array.Exists(conditionResult, result => result));
        }

        onComplete?.Invoke();
    }

    private IEnumerator Run() 
    {
        while (true)
        {
            switch (_mode)
            {
                case RunnerMode.�̼���:
                    yield break;

                case RunnerMode.����_��_����:
                    yield return ExecuteEvents();

                    if(gameObject != null)
                    {
                        Destroy(this.gameObject);
                    }
                    yield break;

                case RunnerMode.����_��_��Ȱ��ȭ:
                    yield return ExecuteEvents();

                    gameObject.SetActive(false);
                    yield break;

                case RunnerMode.����_��_���:
                    yield return ExecuteEvents();
                    yield return CoroutineDelegator.Instance.ExecuteCoroutine(
                        _RunnerID + "CheckCondition", 
                        CheckCondition(
                            _customStartConditions,
                            _necessaryAndSufficientStartCondition,
                            () => CoroutineDelegator.Instance.ExecuteCoroutine(_RunnerID + "Run", Run())
                        ));

                    yield break;
            }
        }
    }

    private IEnumerator ExecuteEvents()
    {
        for(int i = 0; i< _customEvents.Count; i++)
        {
            bool isCompleted = false;
            if (_customEvents[i] != null)
            {
                _customEvents[i].ExecuteEvent(() => isCompleted = true);

                yield return new WaitUntil(() => isCompleted);
            }
        }
    }
}
