using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // deltaTime 계산: 각 프레임에 걸린 시간
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS 계산
        float fps = 1.0f / deltaTime;

        // GUI 스타일 설정 (폰트 크기, 색상 등)
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // FPS를 화면의 왼쪽 상단에 표시
        GUI.Label(new Rect(10, 10, 200, 30), $"FPS: {Mathf.Ceil(fps)}", style);
    }
}
