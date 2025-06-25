using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    public enum RunnerMode
    {
        미선택,
        실행_후_종료,
        실행_후_대기,
        실행_후_비활성화
    }

    public string _RunnerID = "";

    [Header("러너의 동작 모드")]
    [SerializeField] private RunnerMode _mode = RunnerMode.미선택;

    [Space(20f), Header("시작 조건 설정")]
    public List<CustomCondition> _customStartConditions;

    [Header("모든 조건을 만족해야 통과")]
    public bool _necessaryAndSufficientStartCondition = false;

    [Space(20f), Header("실행할 이벤트 설정"), Tooltip("설정한 모든 Event들을 위에서 아래로 순서대로 실행합니다.")]
    public List<CustomEvent> _customEvents;

    [Space(20f), Header("종료 조건 설정")]
    public List<CustomCondition> _customEndConditions;

    [Header("모든 조건을 만족해야 통과")]
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
                case RunnerMode.미선택:
                    yield break;

                case RunnerMode.실행_후_종료:
                    yield return ExecuteEvents();

                    if(gameObject != null)
                    {
                        Destroy(this.gameObject);
                    }
                    yield break;

                case RunnerMode.실행_후_비활성화:
                    yield return ExecuteEvents();

                    gameObject.SetActive(false);
                    yield break;

                case RunnerMode.실행_후_대기:
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
