using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    private enum ManagerList
    {
        DataManager, CoroutineDelegator, GameManager, SceneManager, InputManager, NavGridManager, SoundManager, MaxCount
    }

    public List<ManagerBase> _managerClasses;
    private short _order = -1;

    private void Awake()
    {
        EventManager.Instance.AddListener(EventList.EManagerAwake, AwakeManager);

        if (_managerClasses == null)
        {
            Debug.LogError("ManagerClass List is Null");
            return;
        }

        Application.targetFrameRate = 60;
        AwakeManager(null);
    }

    private void AwakeManager(object param)
    {
        if (_order < (int)ManagerList.MaxCount - 1)
        {
            _order++;
            _managerClasses[_order].gameObject.SetActive(true);
        }
        else Destroy(this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventList.EManagerAwake, AwakeManager);
        EventManager.Instance.TriggerEvent(EventList.EManagerAwake, true);
    }
}
