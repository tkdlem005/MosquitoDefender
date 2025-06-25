using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{
    Speed,
    Gas,
    Horn
}

public class AbilityManager : MonoBehaviour
{
    public Button _speedButton;
    public Button _gasButton;
    public Button _hornButton;

    private void Start()
    {
        _speedButton.onClick.AddListener(() => OnAbilitySelected(AbilityType.Speed));
        _gasButton.onClick.AddListener(() => OnAbilitySelected(AbilityType.Gas));
        _hornButton.onClick.AddListener(() => OnAbilitySelected(AbilityType.Horn));
    }

    private void OnAbilitySelected(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Speed:
                PlayerCharacter.Instance.IncreaseStatus(AbilityType.Speed);
                break;
            case AbilityType.Gas:
                PlayerCharacter.Instance.IncreaseStatus(AbilityType.Gas);
                break;
            case AbilityType.Horn:
                PlayerCharacter.Instance.IncreaseStatus(AbilityType.Horn);
                break;
        }

        DisableAbilityButtons();

        EventManager.Instance.TriggerEvent(EventList.ESceneChangeStart, SceneState.LOADING);
    }

    private void DisableAbilityButtons()
    {
        _speedButton.interactable = false;
        _gasButton.interactable = false;
        _hornButton.interactable = false;
    }
}
