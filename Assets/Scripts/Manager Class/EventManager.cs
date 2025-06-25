using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventList
{
    None,
    EManagerAwake,
    EUpdateGasGauge,
    EGameStart,
    EUpdateTimer,
    EGameWin,
    EGameLose,
    ESceneChangeStart,
    ESceneChangeEnd
}

public class EventManager
{
    private Dictionary<EventList, Action<object>> _eventDictionary;
    private static EventManager _eventManager;

    public static EventManager Instance
    {
        get
        {
            if (_eventManager == null)
            {
                _eventManager = new EventManager();
                _eventManager._eventDictionary = new Dictionary<EventList, Action<object>>();
            }
            return _eventManager;
        }
    }

    public void AddListener(EventList eventName, Action<object> listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent += listener;
            _eventDictionary[eventName] = thisEvent;
        }
        else
            _eventDictionary.Add(eventName, listener);
    }

    public void RemoveListener(EventList eventName, Action<object> listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;

            if (thisEvent == null)
                _eventDictionary.Remove(eventName);
            else
                _eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(EventList eventName, object param = null)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            thisEvent?.Invoke(param);
    }
}