using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_SpawnChildren : CustomEvent
{
    private GameObject _childrenPrefab = Resources.Load<GameObject>("Prefabs/Children");
    private Sprite[] _childerenSprite = new Sprite[] { Resources.Load<Sprite>("Prefabs/Children") };
    public override void ExecuteEvent(Action action)
    {
        GameObject children = Instantiate(_childrenPrefab);
        children.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer);
        //spriteRenderer.sprite;
        children.transform.position = transform.parent.transform.position;
        children.SetActive(true);
    }
}
