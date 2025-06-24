using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[Serializable]
public abstract class CustomEvent : MonoBehaviour
{
    public abstract void ExecuteEvent(Action action);
}
