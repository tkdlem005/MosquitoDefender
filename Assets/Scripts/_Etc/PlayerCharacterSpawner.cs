using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public static bool bWasActioned = false;

    public GameObject _playerPrefabs;

    private void Awake()
    {
        if (bWasActioned)
        {
            Destroy(gameObject);
            return;
        }
            
        Instantiate(_playerPrefabs);
        bWasActioned = true;
    }
}
