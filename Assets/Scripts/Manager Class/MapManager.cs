using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : ManagerBase
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private GameObject _mapPrefab;
    private GameObject _mapObj = null;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ESettingMap, MapSetting);

        InitializeEnd();
    }

    protected override void ResetManager(object param)
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        _mapObj = null;
    }

    private void MapSetting(object param)
    {
        if (param is StageData stageData)
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
                _mapObj = null;
            }

            _mapPrefab = stageData._mapPrefabs;

            _mapObj = Instantiate(_mapPrefab);
            _mapObj.transform.SetParent(transform, false);
            _mapObj.transform.position = Vector3.zero;

            EventManager.Instance.TriggerEvent(EventList.EMapSettingDone);
        }

        else return;
    }
}
