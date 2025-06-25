using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    [Header("스테이지 시간")]
    public float _stageTime = 0.0f;

    [Space(20f), Header("맵 정보")] 
    public NavGridData _mapData;
    public GameObject _mapPrefabs;

    [Space(20f), Header("카메라 세팅")]
    public List<CameraSettings> _cameraSettings;
}
