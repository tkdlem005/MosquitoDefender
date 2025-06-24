using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineDelegator : ManagerBase
{
    public static CoroutineDelegator Instance { get; private set; }

    public Dictionary<string, Coroutine> _coroutineDictionary = new Dictionary<string, Coroutine>();

    private void Awake() => Initialize();

    protected override void Initialize()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        InitializeEnd();
    }

    public Coroutine ExecuteCoroutine(string id, IEnumerator routine)
    {
        if(_coroutineDictionary.TryGetValue(id, out var existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
            _coroutineDictionary.Remove(id);
        }

        Coroutine newCoroutine = StartCoroutine(RunAndRemove(id, routine));
        _coroutineDictionary[id] = newCoroutine;

        return newCoroutine;
    }

    public void StopDelegatedCoroutine(string id)
    {
        if (_coroutineDictionary.TryGetValue(id, out var coroutine))
        {
            StopCoroutine(coroutine);
            _coroutineDictionary.Remove(id);
        }
    }

    public void InterruptAllCoroutines()
    {
        foreach (var coroutine in _coroutineDictionary.Values)
        {
            StopCoroutine(coroutine);
        }
        _coroutineDictionary.Clear();
    }

    private IEnumerator RunAndRemove(string id, IEnumerator routine)
    {
        yield return StartCoroutine(routine);
        _coroutineDictionary.Remove(id);
    }
}
