using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Camera _mainCamera;

    [SerializeField, Range(0f, 10f)]
    private float followSpeed = 5f;

    [SerializeField, Range(0f, 10f)]
    private float rotationSpeed = 5f;

    [field: SerializeField] public Transform TargetTransform { get; private set; }
    [field: SerializeField] public Vector3 PositionOffset { get; private set; }     // 카메라와 플레이어 사이의 거리

    [field: SerializeField] public float MinX { get; private set; }            // 카메라의 x축 최소 위치
    [field: SerializeField] public float MaxX { get; private set; }            // 카메라의 x축 최대 위치

    [field: SerializeField] public float MinZ { get; private set; }            // 카메라의 y축 최소 위치
    [field: SerializeField] public float MaxZ { get; private set; }            // 카메라의 y축 최대 위치

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
        if (TargetTransform != null)
        {
            Vector3 targetPosition = TargetTransform.position + TargetTransform.rotation * PositionOffset;

            targetPosition.x = Mathf.Clamp(targetPosition.x, MinX, MaxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, MinZ, MaxZ);

            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            Quaternion targetRotation = TargetTransform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    public void SetTarget(Transform targetTransform) => TargetTransform = targetTransform;
}
