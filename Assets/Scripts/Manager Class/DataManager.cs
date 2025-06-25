using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : ManagerBase
{
    public static DataManager Instance { get; private set; }

    [SerializeField] private List<StageData> _stageDatas;

    private void Awake() => Initialize();


    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    public StageData GetStageData(int stage)
    {
        return _stageDatas[stage - 1];
    }
}
