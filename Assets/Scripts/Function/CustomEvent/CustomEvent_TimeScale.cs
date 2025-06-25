using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_TimeScale : CustomEvent
{
    [Range(0.0f, 2.0f)] 
    public float _timeScale = 1f;

    public override void ExecuteEvent(Action action)
    {
        Time.timeScale = _timeScale;

        action?.Invoke();
    }
}
