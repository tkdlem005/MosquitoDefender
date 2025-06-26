using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvnet_StopAllSFX : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopAllSFX();
        }

        action?.Invoke();
    }
}
