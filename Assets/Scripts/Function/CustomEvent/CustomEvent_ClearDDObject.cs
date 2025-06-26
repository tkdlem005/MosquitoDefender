using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_ClearDDObject : CustomEvent
{
    public override void ExecuteEvent(Action action)
    {
        GameObject[] DDObj = GameObject.FindGameObjectsWithTag("DDObj");

        foreach (var elem in DDObj)
        {
            Destroy(elem);
        }

        Destroy(PlayerCharacter.Instance.gameObject);

        action?.Invoke();
    }
}
