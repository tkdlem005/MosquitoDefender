using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_PlayBGM : CustomEvent
{
    public string _soundID = "";

    public override void ExecuteEvent(Action action)
    {
        SoundManager.Instance.PlayBGM(_soundID);

        action?.Invoke();
    }
}
