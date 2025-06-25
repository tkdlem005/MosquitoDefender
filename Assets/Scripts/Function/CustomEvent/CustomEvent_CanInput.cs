using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_CanInput : CustomEvent
{
    public bool _canInput = false;

    public override void ExecuteEvent(Action action)
    {
        InputManager.Instance.CanMove = _canInput;

        action?.Invoke();
    }
}
