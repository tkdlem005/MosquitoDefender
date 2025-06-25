using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Slider _gasGauge;

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
        EventManager.Instance.AddListener(EventList.EUpdateTimer, UpdateGameTimer);
    }

    private void UpdateGasGauge(object param)
    {
        if (_gasGauge == null) return;

        if(param is float ratio) _gasGauge.value = ratio;
    }

    private void UpdateGameTimer(object param)
    {
        if (_timerText == null || param == null) return;

        if (param is float time)
            _timerText.text = time.ToString();
    }
}
