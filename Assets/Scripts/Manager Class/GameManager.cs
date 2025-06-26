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
    private GameState _prevGameState = GameState.None;

    private float _currentTime;
    private bool _isGameOver;

    public float CurrentTime => _currentTime;

    public GameState CurGameState => _gameState;

    public GameState PrevGameState => _prevGameState;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ELoadingStart, LoadNextGame);
        EventManager.Instance.AddListener(EventList.EGameStart, StartGameTimer);

        InitializeEnd();
    }

    protected override void ResetManager(object param)
    {
        _gameState = GameState.None;

        _currentTime = 0f;

        _isGameOver = false;

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    public void LoadNextGame(object param)
    {
        Debug.Log("LoadNextGame");

        if(_gameState == GameState.Clear)
        {
            _gameState = _prevGameState;
        }

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
            if (cell._bIsWalkable && !cell._bIsClean)
            {
                UpdateCleanProgressUI();
                return;
            }
        }

        _isGameOver = true;
        EventManager.Instance.TriggerEvent(EventList.EStageEnd);
        if (_gameState == GameState.Stage3)
        {
            _gameState = GameState.Ending;
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.END);
        }
        else
        {
            _gameState = GameState.Clear;
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.CLEAR);
        }

        if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
    }

    private void OnStageOver()
    {
        if (_isGameOver) return;

        CheckAllCellsClean();

        if (!_isGameOver)
        {
            _isGameOver = true;

            _prevGameState = _gameState;
            _gameState = GameState.Fail;

            EventManager.Instance.TriggerEvent(EventList.EStageEnd);
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.FAIL);

            _isGameOver = false;
        }
    }

    public void UpdateCleanProgressUI()
    {
        if (NavGridManager.Instance == null || HUDManager.Instance == null) return;

        var grid = NavGridManager.Instance.GetGrid();

        int totalWalkable = 0;
        int cleaned = 0;

        foreach (var cell in grid.Values)
        {
            if (cell._bIsWalkable)
            {
                totalWalkable++;
                if (cell._bIsClean) cleaned++;
            }
        }

        float ratio = totalWalkable > 0 ? (float)cleaned / totalWalkable : 0f;

        HUDManager.Instance.UpdateCleanPercent(cleaned, totalWalkable, ratio);
    }

    private IEnumerator GameTimerRoutine(float StageLimitTime)
    {

        _currentTime = StageLimitTime;
        EventManager.Instance.TriggerEvent(EventList.EUpdateTimer, _currentTime);

        float elapsedTime = 0f;

        while (_currentTime > 0f && !_isGameOver)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 1f)
            {
                int secondsToDecrease = Mathf.FloorToInt(elapsedTime);
                _currentTime -= secondsToDecrease;
                elapsedTime -= secondsToDecrease;

                EventManager.Instance.TriggerEvent(EventList.EUpdateTimer, _currentTime);

                CheckAllCellsClean();
            }

            yield return null;
        }

        if (!_isGameOver)
            OnStageOver();
    }
}
