using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegatorInitializer : MonoBehaviour
{
    public List<GameObject> _gameObjects;

    private void Awake()
    {
        CoroutineDelegator.Instance.StopAllCoroutines();

        foreach (var gameOBJ in _gameObjects)
        {
            gameOBJ.SetActive(true);
        }

        Destroy(this.gameObject);
    }
}
