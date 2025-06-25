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

    public void PlayBGM(int id)
    {
        AudioClip clip = _soundDataList[id]._clip;
        if (_bgmSource.clip == clip) return;
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }
}
