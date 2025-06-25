using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : ManagerBase
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<SoundData> _bgmSoundDataList;
    [SerializeField] private List<SoundData> _sfxSoundDataList;

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _disInfectionSource;

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

        _disInfectionSource = gameObject.AddComponent<AudioSource>();
        _disInfectionSource.loop = false;

        InitializeEnd();
    }

    public void PlayBGM(int id)
    {
        if (id >= 0 && id < _bgmSoundDataList.Count)
        {
            AudioClip clip = _bgmSoundDataList[id]._clip;

            if (_bgmSource.clip == clip) return;

            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
    }

    public void PlaySFX(int id)
    {
        if(id == 1 || id == 2)
        {
            AudioClip clip = _sfxSoundDataList[id]._clip;

            if (_disInfectionSource.isPlaying) return;

            _disInfectionSource.clip = clip;
            _disInfectionSource.Play();
        }
        else if (id >= 0 && id < _sfxSoundDataList.Count)
        {
            AudioClip clip = _sfxSoundDataList[id]._clip;
            _sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX index {id} out of range!");
        }
    }
}
