using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_StageStart : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        int stageID = -1;

        switch (GameManager.Instance.CurGameState)
        {
            case GameState.Stage1:
                stageID = 1;
                break;

            case GameState.Stage2:
                stageID = 2;
                break;

            case GameState.Stage3:
                stageID = 3;
                break;

            default:
                return;
        }

        float limitTime = DataManager.Instance.GetStageData(stageID)._stageTime;

        EventManager.Instance.TriggerEvent(EventList.EGameStart, limitTime);

        action?.Invoke();
    }
}
