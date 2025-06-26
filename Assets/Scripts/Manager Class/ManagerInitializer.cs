using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    public static bool bIsCreate = false;

    [Header("매니저 프리팹들을 순서대로 넣으세요")]
    public List<GameObject> _managerPrefabs;

    public RectTransform _rectTransform;

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
            Transform parentTransform = transform; // 기본 부모

            if (i == _managerPrefabs.Count - 1)
            {
                // 마지막 매니저 프리팹은 _rectTransform 하위에 생성
                if (_rectTransform != null)
                {
                    parentTransform = _rectTransform;
                }
                else
                {
                    Debug.LogWarning("_rectTransform이 null입니다. 기본 부모로 생성됩니다.");
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
