using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_PlayerPositionSetter : CustomEvent
{
    public Vector3[] _position = new Vector3[3];

    public override void ExecuteEvent(Action action)
    {
        GameState gameState = GameManager.Instance.CurGameState;

        if (gameState == GameState.Clear)
        {
            gameState = GameManager.Instance.PrevGameState;
        }

        switch (gameState)
        {
            case GameState.None:
                PlayerCharacter.Instance.transform.position = _position[0];
                PlayerCharacter.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case GameState.Stage1:
                PlayerCharacter.Instance.transform.position = _position[1];
                PlayerCharacter.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case GameState.Stage2:
                PlayerCharacter.Instance.transform.position = _position[2];
                PlayerCharacter.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            default:
                break;
        }

        action?.Invoke();
    }
}
