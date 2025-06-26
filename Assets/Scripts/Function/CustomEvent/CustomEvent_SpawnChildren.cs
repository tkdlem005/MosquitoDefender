using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomEvent_SpawnChildren : CustomEvent
{
    public float _delayTime = 0.3f;

    private GameObject _childrenPrefab;
    private Sprite[] _childrenSprites;

    public override void ExecuteEvent(Action action) 
        => CoroutineDelegator.Instance.ExecuteCoroutine(EventWaiter(action));

    private IEnumerator EventWaiter(Action action)
    {
        yield return new WaitForSeconds(_delayTime);

        GameObject children = Instantiate(Resources.Load<GameObject>("Prefabs/Children"));
        _childrenSprites = Resources.LoadAll<Sprite>("Sprites/ChildrenSheet");

        children.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer);

        if (spriteRenderer != null && _childrenSprites.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _childrenSprites.Length);
            spriteRenderer.sprite = _childrenSprites[randomIndex];
        }

        children.transform.position = transform.parent.transform.position;

        SoundManager.Instance.PlaySFX(4);
        children.SetActive(true);

        action?.Invoke();
    }
}
