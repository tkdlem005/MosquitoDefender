using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    public static bool bIsCreate = false;

    [Header("�Ŵ��� �����յ��� ������� ��������")]
    public List<GameObject> _managerPrefabs;

    public RectTransform _rectTransform;

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

    private void CreateManagerInstances()
    {
        for (int i = 0; i < _managerPrefabs.Count; i++)
        {
            Transform parentTransform = transform; // �⺻ �θ�

            if (i == _managerPrefabs.Count - 1)
            {
                // ������ �Ŵ��� �������� _rectTransform ������ ����
                if (_rectTransform != null)
                {
                    parentTransform = _rectTransform;
                }
                else
                {
                    Debug.LogWarning("_rectTransform�� null�Դϴ�. �⺻ �θ�� �����˴ϴ�.");
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
