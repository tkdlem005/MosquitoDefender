using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_ClearDDObject : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        GameObject DDObj = GameObject.FindWithTag("DDObj");
        Destroy(DDObj);
        Destroy(PlayerCharacter.Instance.gameObject);

        action?.Invoke();
    }
}
