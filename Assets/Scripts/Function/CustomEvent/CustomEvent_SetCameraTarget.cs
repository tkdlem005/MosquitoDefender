using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_SetCameraTarget : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        Camera.main.TryGetComponent<MainCamera>(out MainCamera mainCamera);

        if(mainCamera != null)
        {
            mainCamera.SetTarget(PlayerCharacter.Instance.transform);
        }

        action?.Invoke();
    }
}
