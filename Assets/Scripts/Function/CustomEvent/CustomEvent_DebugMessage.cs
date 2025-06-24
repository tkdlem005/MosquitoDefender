using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_DebugMessage : CustomEvent
{
    public string _debugMessage = "";

    public override void ExecuteEvent(Action action)
    {
        Debug.Log(_debugMessage);

        action?.Invoke();
    }
}
