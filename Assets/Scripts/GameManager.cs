using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string mainMenuSceneName;
    public string playerSceneName;
    public string[] startGameScenes;
    LevelManager lvlMang;

    public void Awake()
    {
        lvlMang = LevelManager.instance;
    }

    public void StartGame()
    {
        lvlMang.SwitchToScene(playerSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
