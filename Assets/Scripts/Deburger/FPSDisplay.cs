using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // deltaTime ���: �� �����ӿ� �ɸ� �ð�
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS ���
        float fps = 1.0f / deltaTime;

        // GUI ��Ÿ�� ���� (��Ʈ ũ��, ���� ��)
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // FPS�� ȭ���� ���� ��ܿ� ǥ��
        GUI.Label(new Rect(10, 10, 200, 30), $"FPS: {Mathf.Ceil(fps)}", style);
    }
}
