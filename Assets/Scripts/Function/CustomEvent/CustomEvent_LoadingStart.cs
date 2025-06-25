using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_LoadingStart : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        EventManager.Instance.TriggerEvent(EventList.ELoadingStart);
        action?.Invoke();
    }
}
