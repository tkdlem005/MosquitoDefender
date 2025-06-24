using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Sound", menuName = "���� �߰�")]
public class SoundData : ScriptableObject
{
    public string _name;
    public AudioClip _clip;
}
