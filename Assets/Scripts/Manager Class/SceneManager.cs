using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneState
{
    TITLE, LOADING, PLAY, CLEAR, FAIL, ABILITYSELECT, END
}

public class SceneManager : ManagerBase
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private SceneState _curSceneState;

    private Coroutine _loadCoroutine;

    private void Awake() => Initialize();

    private void Start() => _curSceneState = SceneState.TITLE;

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ESceneChangeStart, SetSceneState);

        InitializeEnd();
    }

    protected override void ResetManager(object param)
    {
        if (_loadCoroutine != null)
        {
            StopCoroutine(_loadCoroutine);
            _loadCoroutine = null;
        }

        _curSceneState = SceneState.TITLE;
    }

    private void SetSceneState(object param)
    {
        SceneState sceneState = (SceneState)param;

        if (_loadCoroutine != null)
        {
            StopCoroutine(_loadCoroutine);
            _loadCoroutine = null;
        }

        AsyncOperation asyncload = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneState, LoadSceneMode.Single);
        _curSceneState = sceneState;

        _loadCoroutine = StartCoroutine(LoadScene(asyncload));
    }

    public IEnumerator LoadScene(AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        EventManager.Instance.TriggerEvent(EventList.ESceneChangeEnd, _curSceneState);
    }
}
