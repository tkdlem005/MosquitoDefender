using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    public float _stageTime = 0.0f;
    public NavGridData _mapData;
    public List<CameraSettings> _cameraSettings;
}
