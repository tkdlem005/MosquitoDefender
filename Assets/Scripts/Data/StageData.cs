using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("�������� �ð�")]
    public float _stageTime = 0.0f;

    [Space(20f), Header("�� ����")] 
    public NavGridData _mapData;
    public GameObject _mapPrefabs;

    [Space(20f), Header("ī�޶� ����")]
    public List<CameraSettings> _cameraSettings;
}
