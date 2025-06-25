using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEvent_SceneChange : CustomEvent
{
    [Header("������ Scene�� �������ּ���")]
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
