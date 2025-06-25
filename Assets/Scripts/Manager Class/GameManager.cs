using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float _limitTime = 0.0f;
    private float _currentTime;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    private IEnumerator SetTimer()
    {

        yield return null;
    }
}
