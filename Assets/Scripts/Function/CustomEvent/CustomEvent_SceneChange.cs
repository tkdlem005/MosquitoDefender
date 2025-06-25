using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEvent_SceneChange : CustomEvent
{
    [Header("변경할 Scene을 설정해주세요")]
    [SerializeField] private SceneState _sceneState;

    public bool _notUseSceneManager = false;

    public override void ExecuteEvent(Action action)
    {
        if (!_notUseSceneManager)
        {
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, _sceneState);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)_sceneState, LoadSceneMode.Single);
        }

        action?.Invoke();
    }
}
