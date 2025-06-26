using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualViewer : MonoBehaviour
{
    public Image _buttonImage;
    public Sprite[] _sprites;

    private int _count = 0;

    public void OnClick()
    {
        if (_count == 2)
        {
            EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.LOADING);
            return;
        }

        _buttonImage.sprite = _sprites[_count];

        _count++;
    }
}
