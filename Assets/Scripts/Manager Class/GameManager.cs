using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NotPlay,
    Stage1,
    Stage2,
    Stage3,
    Clear,
    Fail
}

public class GameManager : ManagerBase
{
    public static GameManager Instance { get; private set; }

    private Coroutine _timerCoroutine;

    [SerializeField, Space(20f)]
    private GameState _gameState = GameState.NotPlay;

    private float _currentTime;
    private bool _isClear;
    private bool _isGameOver;

    public float CurrentTime => _currentTime;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.EGameStart, StartGameTimer);

        InitializeEnd();
    }

    private void StartGameTimer(object param)
    {
        if(param is float limitTime)
            _timerCoroutine = StartCoroutine(GameTimerRoutine(limitTime));
    }

    private void CheckAllCellsClean()
    {
        if (NavGridManager.Instance == null)
            return;

        var grid = NavGridManager.Instance.GetGrid();

        foreach (var kvp in grid)
        {
            var cell = kvp.Value;
            if (cell._bIsWalkable && !cell._bIsClean) return;
        }

        _isGameOver = true;
        _gameState = GameState.Clear;
        EventManager.Instance.TriggerEvent(EventList.EGameWin);

        if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
    }

    private void OnStageOver()
    {
        if (_isGameOver) return;

        CheckAllCellsClean();

        if (!_isGameOver)
        {
            _isGameOver = true;
            _gameState = GameState.Fail;
            EventManager.Instance.TriggerEvent(EventList.EGameLose, _gameState);
        }
    }

    private IEnumerator GameTimerRoutine(float StageLimitTime)
    {
        _currentTime = StageLimitTime;

        while (_currentTime > 0f && !_isGameOver)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;

            EventManager.Instance.TriggerEvent(EventList.EUpdateTimer, _currentTime);

            CheckAllCellsClean();
        }

        if (!_isGameOver) OnStageOver();
    }
}
