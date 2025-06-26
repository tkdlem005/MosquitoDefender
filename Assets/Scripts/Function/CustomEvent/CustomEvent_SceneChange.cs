using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEvent_SceneChange : CustomEvent
{
    [Header("변경할 Scene을 설정해주세요")]
    [SerializeField] private SceneState _sceneState;

    public override void ExecuteEvent(Action action)
    {
        if (SceneManager.Instance)
        {
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, _sceneState);
        }
        else
        {
            Debug.Log("Choose Not use SceneManager");
            UnityEngine.SceneManagement.SceneManager.LoadScene("00.TITLE", LoadSceneMode.Single);
        }

        action?.Invoke();
    }
}
