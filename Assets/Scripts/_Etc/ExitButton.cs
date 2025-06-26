using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnClick()
    {
        GameObject DDObj = GameObject.FindGameObjectWithTag("DDObj");
        Destroy(DDObj);

        PlayerCharacter.Instance.ResetPlayer();

        EventManager.Instance.TriggerEvent(EventList.EManagerReset);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
