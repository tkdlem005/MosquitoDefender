using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_StageStart : CustomEvent
{
    public int _stageID = 0;

    public override void ExecuteEvent(Action action)
    {
        if (_stageID == 0) return;

        float limitTime = DataManager.Instance.GetStageData(_stageID)._stageTime;

        EventManager.Instance.TriggerEvent(EventList.EGameStart, limitTime);

        action?.Invoke();
    }
}
