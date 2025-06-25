using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomEvent_PlaySFX : CustomEvent
{
    public int _soundID = -1;

    public override void ExecuteEvent(Action action)
    {
        SoundManager.Instance.PlaySFX(_soundID);

        action?.Invoke();
    }
}
