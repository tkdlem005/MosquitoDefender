using System.Collections;
using UnityEngine;

public class Disinfection : MonoBehaviour
{
    private PlayerCharacter _owner;

    [SerializeField] private GameObject _disinfectEffectPrefab;

    [SerializeField] private float _curGasAmount;
    [SerializeField] private float _gasConsumptionRate = 1f;

    [SerializeField] private bool _startAction = false;

    public float CurGasAmount => _curGasAmount;
    public float GasConsumptionRate => _gasConsumptionRate;

    private Coroutine _disinfectCoroutine;

    private void Awake()
    {
        TryGetComponent(out _owner);
    }

    private void OnDisable()
    {
        if (_disinfectCoroutine != null)
        {
            StopCoroutine(_disinfectCoroutine);
        }
    }

    private void Update()
    {
        if (!_startAction || _curGasAmount <= 0f) return;

        Vector3 worldPos = transform.position;
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));

        if (NavGridManager.Instance.TryGetCell(gridPos, out GridCell cell))
        {
            if (!cell._bIsClean)
            {
                cell._bIsClean = true;
                SpawnEffect(cell.GetGridPos());
            }
        }
    }

    public void Execute()
    {
        _curGasAmount = PlayerCharacter.Instance.MaxGasAmount;
        _disinfectCoroutine = StartCoroutine(DisinfectRoutine());

        _startAction = true;
    }

    private void SpawnEffect(Vector3 worldPos)
    {
        if (_disinfectEffectPrefab != null)
        {
            GameObject effect = Instantiate(_disinfectEffectPrefab, worldPos, Quaternion.identity);
        }
            
    }

    public void AddGas(float amount) => _curGasAmount = Mathf.Min(_curGasAmount + amount, PlayerCharacter.Instance.MaxGasAmount);

    public void SetGas(float amount) => _curGasAmount = Mathf.Clamp(amount, 0, PlayerCharacter.Instance.MaxGasAmount);

    private IEnumerator DisinfectRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (_curGasAmount <= 0f) continue;

            _curGasAmount -= _gasConsumptionRate;
            _curGasAmount = Mathf.Max(_curGasAmount, 0f);

            EventManager.Instance.TriggerEvent(EventList.EUpdateGasGauge, _curGasAmount / PlayerCharacter.Instance.MaxGasAmount);
        }
    }
}
