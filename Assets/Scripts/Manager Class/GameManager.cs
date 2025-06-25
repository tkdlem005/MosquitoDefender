using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Stage1,
    Stage2,
    Stage3,
    Clear,
    Fail,
    Ending
}

public class GameManager : ManagerBase
{
    public static GameManager Instance { get; private set; }

    private Coroutine _timerCoroutine;

    [SerializeField, Space(20f)]
    private GameState _gameState = GameState.None;

    private float _currentTime;
    private bool _isClear;
    private bool _isGameOver;

    public float CurrentTime => _currentTime;

    private void Awake() => Initialize();

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventList.ELoadingStart, LoadNextGame);
        EventManager.Instance.RemoveListener(EventList.EGameStart, StartGameTimer);
    }

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ELoadingStart, LoadNextGame);
        EventManager.Instance.AddListener(EventList.EGameStart, StartGameTimer);

        InitializeEnd();
    }

    public void LoadNextGame(object param)
    {
        Debug.Log("LoadNextGame");

        switch (_gameState)
        {
            case GameState.None:
                _gameState = GameState.Stage1;
                EventManager.Instance.TriggerEvent(
                    EventList.ESettingMap,
                    DataManager.Instance.GetStageData(1)
                );
                break;

            case GameState.Stage1:
                _gameState = GameState.Stage2;
                EventManager.Instance.TriggerEvent(
                    EventList.ESettingMap,
                    DataManager.Instance.GetStageData(2)
                );
                break;

            case GameState.Stage2:
                _gameState = GameState.Stage3;
                EventManager.Instance.TriggerEvent(
                    EventList.ESettingMap,
                    DataManager.Instance.GetStageData(3)
                );
                break;

            case GameState.Stage3:
                _gameState = GameState.Ending;
                break;
        }
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
        EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.CLEAR);

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
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.CLEAR);
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
