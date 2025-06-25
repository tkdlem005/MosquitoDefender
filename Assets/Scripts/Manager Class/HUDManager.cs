using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Gas UI")]
    [SerializeField] private Slider _gasGauge;

    [Header("Horn UI")]
    [SerializeField] private GameObject _hornUI;
    [SerializeField] private List<Image> _hornVolumeImages;

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
        EventManager.Instance.AddListener(EventList.EUpdateHornDecibel, UpdateHornDecibel);
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

    private void UpdateHornDecibel(object param)
    {
        if (_hornVolumeImages == null || _hornVolumeImages.Count != 9) return;

        if (param is int decibel)
        {
            ResetHornUI();

            int volumeToActivate = Mathf.Min(decibel / 3, 3);
            for (int i = 0; i < volumeToActivate; i++)
            {
                _hornVolumeImages[i].enabled = true;
            }
        }
    }

    private void ResetHornUI()
    {
        foreach (var image in _hornVolumeImages)
        {
            image.enabled = false;
        }
    }
}
