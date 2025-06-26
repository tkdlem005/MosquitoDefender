using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    private enum ManagerList
    {
        DataManager, CoroutineDelegator, GameManager, SceneManager, InputManager, NavGridManager, SoundManager, MapManager, MaxCount
    }

    public static bool bIsCreate = false;

    [Header("매니저 프리팹들을 순서대로 넣으세요")]
    public List<GameObject> _managerPrefabs;

    // 생성된 매니저 인스턴스들
    private List<GameObject> _managerInstances = new List<GameObject>();

    private short _order = -1;

    private void Awake()
    {
        if (bIsCreate)
        {
            Destroy(gameObject);
            return;
        }
        bIsCreate = true;

        EventManager.Instance.AddListener(EventList.EManagerAwake, AwakeManager);

        if (_managerPrefabs == null || _managerPrefabs.Count == 0)
        {
            Debug.LogError("Manager Prefabs List is Null or Empty");
            return;
        }

        Application.targetFrameRate = 60;

        CreateManagerInstances();

        AwakeManager(null);
    }

    // 프리팹들을 Instantiate 하여 _managerInstances에 담고 모두 비활성화 상태로 둠
    private void CreateManagerInstances()
    {
        foreach (var prefab in _managerPrefabs)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            _managerInstances.Add(instance);
        }
    }

    private void AwakeManager(object param)
    {
        if (_order < _managerInstances.Count - 1)
        {
            _order++;
            _managerInstances[_order].SetActive(true);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListener(EventList.EManagerAwake, AwakeManager);
        EventManager.Instance.TriggerEvent(EventList.EManagerAwake, true);
    }
}
