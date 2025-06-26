using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualViewer : MonoBehaviour
{
    public Image _buttonImage;
    public Sprite _sprite;

    private int _count = 0;

    public void OnClick()
    {
        if (_count == 1)
        {
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.LOADING);
            return;
        }

        _buttonImage.sprite = _sprite;

        _count++;
    }
}
