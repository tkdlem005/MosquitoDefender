using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
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

    private void CreateManagerInstances()
    {
        for (int i = 0; i < _managerPrefabs.Count; i++)
        {
            Transform parentTransform = transform; // 기본 부모: ManagerInitializer 오브젝트

            if (i == _managerPrefabs.Count - 1)
            {
                // 마지막 프리팹은 PlayerHUD.Instance.transform.GetChild(0) 을 부모로 지정
                if (HUDManager.Instance != null && HUDManager.Instance.transform.childCount > 0)
                {
                    parentTransform = HUDManager.Instance.transform.GetChild(0);
                }
                else
                {
                    Debug.LogWarning("HUDManager.Instance 또는 자식이 없습니다. 기본 부모로 생성합니다.");
                }
            }

            GameObject instance = Instantiate(_managerPrefabs[i], parentTransform);
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
