using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class CustomCondition : MonoBehaviour
{
    public abstract void CompleteCondition(Action action);

    public virtual bool IsSatisfied() => false;
}
