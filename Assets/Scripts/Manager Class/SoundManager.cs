using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : ManagerBase
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<SoundData> _soundDataList;

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.volume = 0.5f;
        _bgmSource.loop = true;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;

        InitializeEnd();
    }

    public void PlayBGM(string id)
    {
        for (int i = 0; i < _soundDataList.Count; i++)
        {
            if (_soundDataList[i]._name == id)
            {
                AudioClip clip = _soundDataList[i]._clip;
                if (_bgmSource.clip == clip) return; // 이미 재생 중인 BGM이면 재생하지 않음
                _bgmSource.clip = clip;
                _bgmSource.Play();
                break;
            }
        }
    }
}
