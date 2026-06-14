using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_CanInput : CustomEvent
{
    public bool _canInput = false;

    public override void ExecuteEvent(Action action)
    {
        EventManager.Instance.TriggerEvent(EventList.EInputEnabled, _canInput);

        action?.Invoke();
    }
}
