using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : ManagerBase
{
    public static DataManager Instance { get; private set; }

    [SerializeField] private List<StageData> _stageDatas;

    private void Awake() => Initialize();

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    protected override void ResetManager(object param) { }


    public StageData GetStageData(int stage)
    {
        return _stageDatas[stage - 1];
    }
}
