using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Sound", menuName = "사운드 추가")]
public class SoundData : ScriptableObject
{
    public string _name;
    public AudioClip _clip;
}
