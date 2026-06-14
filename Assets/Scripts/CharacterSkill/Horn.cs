using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    [SerializeField] private GameObject _hornEffectPrefab; // Sprite가 붙은 프리팹
    [SerializeField] private Transform _effectSpawnPoint;
    [SerializeField] private PlayerCharacter _owner;

    [SerializeField] private float _freezeTime = 2f;
    [SerializeField] private int _decibel = 0;
    [SerializeField] private float _decibelDecayTime = 6f;

    [SerializeField] private bool _isLocked = false;
    [SerializeField] private bool _isComplain = false;

    public int Decibel => _decibel;
    public bool IsLocked => _isLocked;
    public bool IsComplain => _isComplain;

    private void Start() => TryGetComponent(out _owner);

    public void Activate()
    {
        if (_isLocked) return;

        SoundManager.Instance.PlaySFX(3);
        ShowHornEffect();
        HonkHorn();
    }

    public void SetHornEnabled(bool state)
    {
        _isLocked = !state;
    }

    public void SetComplainEventEnd() => _isComplain = false;

    private void ShowHornEffect()
    {
        if (_hornEffectPrefab == null) return;

        Vector3 spawnPos = _effectSpawnPoint != null ? _effectSpawnPoint.position : transform.position;

        GameObject effect = Instantiate(_hornEffectPrefab, spawnPos, _hornEffectPrefab.transform.rotation);

        Destroy(effect, 0.3f);
    }

    private void HonkHorn()
    {
        // Children 감지시 Freeze 적용
        Collider[] hits = Physics.OverlapSphere(transform.position, _owner.HornRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IFreezable freezeable) && !hit.CompareTag("Player"))
            {
                freezeable.Freeze(_freezeTime);
            }
        }

        _decibel += 3;

        if (_decibel == 3) StartCoroutine(DecayDecibel());

        EventManager.Instance.TriggerEvent(EventList.EUpdateHornDecibel, _decibel);

        // Decibel 임계 이상 시 항의전화 Event 작동
        if (_decibel >= 9)
        {
            _isComplain = true;
            _owner.Freeze(3f);
            EventManager.Instance.TriggerEvent(EventList.EOnDecibelPenalty);
            _decibel = 0;
        }
    }

    private IEnumerator DecayDecibel()
    {
        while (_decibel > 0)
        {
            yield return new WaitForSeconds(_decibelDecayTime);
            _decibel = Mathf.Max(0, _decibel - 1);

            EventManager.Instance.TriggerEvent(EventList.EUpdateHornDecibel, _decibel);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _owner.HornRadius);
    }
}
