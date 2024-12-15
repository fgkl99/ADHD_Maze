using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject paseUI;

    public void onGamePausePress()
    {
        paseUI.SetActive(true);
        Debug.Log("Bottone premuto");

    }
    public void onGameResumePress()
    {
        paseUI.SetActive(false);
        Debug.Log("Bottone premuto");

    }

    public void onGameExitPress()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
