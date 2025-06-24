using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCondition_Delay : CustomCondition
{
    [SerializeField] private float _waitSeconds = 0.0f;

    public CustomCondition_Delay(float time) => _waitSeconds = time;

    public override void CompleteCondition(Action action)
    {
        CoroutineDelegator.Instance.StartCoroutine(Delayer(action));
    }

    private IEnumerator Delayer(Action action)
    {
        yield return new WaitForSeconds(_waitSeconds);
        action?.Invoke();
    }
}
