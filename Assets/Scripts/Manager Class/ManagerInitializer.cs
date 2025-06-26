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

    [Header("�Ŵ��� �����յ��� ������� ��������")]
    public List<GameObject> _managerPrefabs;

    // ������ �Ŵ��� �ν��Ͻ���
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

    // �����յ��� Instantiate �Ͽ� _managerInstances�� ��� ��� ��Ȱ��ȭ ���·� ��
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
