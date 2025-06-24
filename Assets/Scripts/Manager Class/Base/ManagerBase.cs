using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    protected abstract void Initialize();

    protected virtual void InitializeEnd()
    {
        EventManager.Instance.TriggerEvent(EventList.EManagerAwake);
    }
}
