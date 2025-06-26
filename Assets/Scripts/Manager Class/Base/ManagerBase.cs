using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    protected abstract void Initialize();

    protected abstract void ResetManager(object param);

    protected virtual void InitializeEnd()
    {
        EventManager.Instance.AddListener(EventList.EManagerReset, ResetManager);
        EventManager.Instance.TriggerEvent(EventList.EManagerAwake);
    }
}
