using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState
{
    TITLE, 
}

public class SceneManager : ManagerBase
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private SceneState _curSceneState;

    private void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    private void Start() => _curSceneState = SceneState.TITLE;

}
