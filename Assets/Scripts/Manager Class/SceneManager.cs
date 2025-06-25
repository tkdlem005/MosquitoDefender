using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneState
{
    TITLE, LOADING, PLAY, CLEAR, FAIL, END
}

public class SceneManager : ManagerBase
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private SceneState _curSceneState;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        EventManager.Instance.AddListener(EventList.ESceneChangeStart, SetSceneState);

        InitializeEnd();
    }

    private void Start() => _curSceneState = SceneState.TITLE;

    private void SetSceneState(object param)
    {
        SceneState sceneState = (SceneState)param;

        AsyncOperation asyncload = null;

        switch (sceneState)
        {
            case SceneState.CLEAR or SceneState.FAIL:
                break;

            default:
                asyncload = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneState, LoadSceneMode.Single);
                _curSceneState = sceneState;
                break;
        }

        StartCoroutine(LoadScene(asyncload));
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
