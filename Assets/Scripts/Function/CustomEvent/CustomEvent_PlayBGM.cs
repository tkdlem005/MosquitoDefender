using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_PlayBGM : CustomEvent
{
    public int _soundID = -1;

    public override void ExecuteEvent(Action action)
    {
        SoundManager.Instance.PlayBGM(_soundID);

        action?.Invoke();
    }
}
