using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    public Slider _gasGauge;

    private void Awake() => Initialize();

    private void Start()
    {
        _gasGauge.value = 1;
    }

    private void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);

        EventManager.Instance.AddListener(EventList.EUpdateGasGauge, UpdateGasGauge);
    }

    private void UpdateGasGauge(object param)
    {
        if (_gasGauge == null) return;

        if(param is float ratio) _gasGauge.value = ratio;
    }
}
