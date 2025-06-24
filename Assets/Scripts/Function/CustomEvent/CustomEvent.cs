using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public abstract class CustomEvent : MonoBehaviour
{
    public abstract void ExecuteEvent(Action action);
}
