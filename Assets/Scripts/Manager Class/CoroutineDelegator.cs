using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineDelegator : ManagerBase
{
    public static CoroutineDelegator Instance { get; private set; }

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    protected override void ResetManager(object param)
    {
        StopAllCoroutines();
    }

    public Coroutine ExecuteCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}
