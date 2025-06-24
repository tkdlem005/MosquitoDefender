using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance { get; private set; }

    public Camera _mainCamera;

    [field: SerializeField] public Transform TargetTransform { get; private set; }
    [field: SerializeField] public Vector3 PositionOffset { get; private set; }     // ī�޶�� �÷��̾� ������ �Ÿ�

    [field: SerializeField] public float MinX { get; private set; }            // ī�޶��� x�� �ּ� ��ġ
    [field: SerializeField] public float MaxX { get; private set; }            // ī�޶��� x�� �ִ� ��ġ

    [field: SerializeField] public float MinZ { get; private set; }            // ī�޶��� y�� �ּ� ��ġ
    [field: SerializeField] public float MaxZ { get; private set; }            // ī�޶��� y�� �ִ� ��ġ

    [field: SerializeField] public bool Follow { get; set; }

    private void Awake() => Initialize();

    private void Start() => Follow = true;

    private void LateUpdate()
    {
        if (Follow)
            CameraFollow();
    }

    private void Initialize()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (TryGetComponent(out _mainCamera))
            _mainCamera = Camera.main;

        var cameraRect = _mainCamera.rect;
        var scaleheight = ((float)Screen.width / Screen.height) / (1920f / 1080f);
        var scalewidth = 1f / scaleheight;

        if (scaleheight < 1f)
        {
            cameraRect.height = scaleheight;
            cameraRect.y = (1f - scaleheight) / 2f;
        }

        else
        {
            cameraRect.width = scalewidth;
            cameraRect.x = (1f - scalewidth) / 2f;
        }

        _mainCamera.rect = cameraRect;
    }

    private void CameraFollow()
    {
        if(TargetTransform != null)
        {
            Vector3 cameraPosition;
            cameraPosition.x = Mathf.Clamp(TargetTransform.position.x, MinX, MaxX);
            cameraPosition.y = PositionOffset.y;
            cameraPosition.z = Mathf.Clamp(TargetTransform.position.z, MinZ, MaxZ);

            transform.position = cameraPosition + PositionOffset;
        }
    }
}
