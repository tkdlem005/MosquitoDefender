using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Clean UI")]
    [SerializeField] private TextMeshProUGUI _cleanPercentText;

    [Header("Gas UI")]
    [SerializeField] private Slider _gasGauge;

    [Header("Horn UI")]
    [SerializeField] private GameObject _hornUI;
    [SerializeField] private List<Image> _hornVolumeImages;

    [Header("HUD Move UI")]
    [SerializeField] private RectTransform _hudMoveImage;
    
    [Header("OverDecibel HUD UI")]
    [SerializeField] private RectTransform _overDecibelImage;

    [Header("Common")]
    private Vector2 _startPos = new Vector2(-600f, 0f);
    private Vector2 _endPos = new Vector2(50f, 0f);
    private float _moveDuration = 0.2f;

    private void Awake() => Initialize();

    private void Start()
    {
        _gasGauge.value = 1;
        if (_hornUI != null) _hornUI.SetActive(true);
        ResetHornUI();

        if (_hudMoveImage != null)
            _hudMoveImage.anchoredPosition = _startPos;

        if (_overDecibelImage != null)
            _overDecibelImage.anchoredPosition = _startPos;
    }

    private void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);

        EventManager.Instance.AddListener(EventList.EStageEnd, ResetHUD);
        EventManager.Instance.AddListener(EventList.EUpdateGasGauge, UpdateGasGauge);
        EventManager.Instance.AddListener(EventList.EUpdateTimer, UpdateGameTimer);
        EventManager.Instance.AddListener(EventList.EUpdateHornDecibel, UpdateHornDecibel);
    }

    private void ResetHUD(object param)
    {
        if (_timerText != null)
            _timerText.text = "0";

        if (_gasGauge != null)
            _gasGauge.value = 1;

        ResetHornUI();

        if (_hudMoveImage != null)
            _hudMoveImage.anchoredPosition = _startPos;
    }

    private void UpdateGasGauge(object param)
    {
        if (_gasGauge == null) return;

        if (param is float ratio) _gasGauge.value = ratio;
    }

    private void UpdateGameTimer(object param)
    {
        if (_timerText == null || param == null) return;

        if (param is float time)
            _timerText.text = time.ToString();
    }

    public void UpdateCleanPercent(int cleaned, int total, float ratio)
    {
        if (_cleanPercentText == null) return;

        int percent = Mathf.FloorToInt(ratio * 100f);
        _cleanPercentText.text = $"{percent}%";
    }


    private void UpdateHornDecibel(object param)
    {
        if (_hornVolumeImages == null || _hornVolumeImages.Count != 9) return;

        if (param is int decibel)
        {
            ResetHornUI();

            int volumeToActivate = decibel;
            for (int i = 0; i < volumeToActivate; i++)
            {
                _hornVolumeImages[i].enabled = true;
            }
        }
    }

    private void ResetHornUI()
    {
        foreach (var image in _hornVolumeImages)
        {
            image.enabled = false;
        }
    }

    public void OnPlayerCollision() => StartCoroutine(MoveHUDImageToEnd());

    public void OnOverDecibel() => StartCoroutine(MoveOverDecibelImage());

    private IEnumerator MoveHUDImageToEnd()
    {
        float elapsedTime = 0f;
        Vector2 startingPos = _hudMoveImage.anchoredPosition;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;
            _hudMoveImage.anchoredPosition = Vector2.Lerp(startingPos, _endPos, elapsedTime / _moveDuration);
            yield return null;
        }

        _hudMoveImage.anchoredPosition = _endPos;

        yield return new WaitForSeconds(3f - (_moveDuration + _moveDuration / 2));

        StartCoroutine(MoveHUDImageToStart());
    }

    private IEnumerator MoveHUDImageToStart()
    {
        float elapsedTime = 0f;
        Vector2 startingPos = _hudMoveImage.anchoredPosition;

        while (elapsedTime < _moveDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            _hudMoveImage.anchoredPosition = Vector2.Lerp(startingPos, _startPos, elapsedTime / _moveDuration);
            yield return null;
        }

        _hudMoveImage.anchoredPosition = _startPos;
    }

    private IEnumerator MoveOverDecibelImage()
    {
        float elapsedTime = 0f;
        Vector2 startingPos = _overDecibelImage.anchoredPosition;

        // 슬라이드 인
        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;
            _overDecibelImage.anchoredPosition = Vector2.Lerp(startingPos, _endPos, elapsedTime / _moveDuration);
            yield return null;
        }

        _overDecibelImage.anchoredPosition = _endPos;

        // 화면에 잠시 유지
        yield return new WaitForSeconds(3f - (_moveDuration + _moveDuration / 2));

        // 슬라이드 아웃
        elapsedTime = 0f;
        startingPos = _overDecibelImage.anchoredPosition;
        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;
            _overDecibelImage.anchoredPosition = Vector2.Lerp(startingPos, _startPos, elapsedTime / _moveDuration);
            yield return null;
        }

        _overDecibelImage.anchoredPosition = _startPos;
    }
}
