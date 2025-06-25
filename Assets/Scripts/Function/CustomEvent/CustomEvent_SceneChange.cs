using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_SceneChange : CustomEvent
{
    [Header("변경할 Scene을 설정해주세요")]
    [SerializeField] private SceneState _sceneState;

    public override void ExecuteEvent(Action action)
    {
        EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, _sceneState);
        action?.Invoke();
    }
}
