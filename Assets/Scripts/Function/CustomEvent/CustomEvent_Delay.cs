using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvent_Delay : CustomEvent
{
    public float _time = 0.0f;

    private Coroutine _coroutine;

    public override void ExecuteEvent(Action action) => _coroutine = StartCoroutine(EventWaiter(action));

    IEnumerator EventWaiter(Action action)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _time)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        action?.Invoke();
    }
}
