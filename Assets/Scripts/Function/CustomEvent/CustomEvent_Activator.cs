using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_Activator : CustomEvent
{
    public GameObject _targetObject;
    public bool _isActive = false;

    public override void ExecuteEvent(Action action)
    {
        if(_targetObject != null)
        {
            _targetObject.SetActive(_isActive);
        }

        action?.Invoke();
    }
}
