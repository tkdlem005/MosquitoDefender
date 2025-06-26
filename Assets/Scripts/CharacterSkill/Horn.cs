using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    [SerializeField] private GameObject _hornEffectPrefab; // Sprite가 붙은 프리팹
    [SerializeField] private Transform _effectSpawnPoint;

    public float _freezeTime = 2f;
    public int _decibel = 0;
    public float _decibelDecayTime = 6f;

    private bool _isLocked = false;

    public void Activate()
    {
        if (_isLocked) return;

        SoundManager.Instance.PlaySFX(3);
        ShowHornEffect();
        StartCoroutine(HornRoutine());
    }

    private void ShowHornEffect()
    {
        if (_hornEffectPrefab == null) return;

        Vector3 spawnPos = _effectSpawnPoint != null ? _effectSpawnPoint.position : transform.position;

        GameObject effect = Instantiate(_hornEffectPrefab, spawnPos, _hornEffectPrefab.transform.rotation);

        Destroy(effect, 0.3f);
    }

    private IEnumerator HornRoutine()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, PlayerCharacter.Instance.HornRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IFreezable freezeAble))
                freezeAble.Freeze(_freezeTime);
        }

        _decibel += 3;

        EventManager.Instance.TriggerEvent(EventList.EUpdateHornDecibel, _decibel);

        if (_decibel == 3) StartCoroutine(DecayDecibel());

        if (_decibel >= 9)
        {
            InputManager.Instance.CanMove = false;
            _isLocked = true;

            if (HUDManager.Instance != null)
                HUDManager.Instance.OnOverDecibel();

            yield return new WaitForSeconds(3f);

            InputManager.Instance.CanMove = true;
            _decibel = 0;
            _isLocked = false;
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
        Gizmos.DrawWireSphere(transform.position, PlayerCharacter.Instance.HornRadius);
    }
}
