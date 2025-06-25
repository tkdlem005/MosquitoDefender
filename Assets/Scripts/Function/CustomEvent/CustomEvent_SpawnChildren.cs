using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_SpawnChildren : CustomEvent
{
    private GameObject _childrenPrefab = Resources.Load<GameObject>("Prefabs/Children");
    private Sprite[] _childrenSprites = Resources.LoadAll<Sprite>("Sprites/ChildrenSheet");
    public override void ExecuteEvent(Action action)
    {
        GameObject children = Instantiate(_childrenPrefab);
        children.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer);

        if (spriteRenderer != null && _childrenSprites.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _childrenSprites.Length);
            spriteRenderer.sprite = _childrenSprites[randomIndex];
        }

        children.transform.position = transform.parent.transform.position;
        children.SetActive(true);
    }
}
