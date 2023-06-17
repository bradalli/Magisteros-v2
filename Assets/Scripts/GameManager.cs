using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum gameState { MainMenu, Pause, Loading, Gameplay }
    [Header("States")]
    public gameState currGameState;
    public bool paused;

    [Header("Scene names")]
    public string mainMenuSceneName;
    public string playerSceneName;
    public string[] startGameScenes;
    LevelManager lvlMang;

    [Header("Events")]
    public UnityEvent startPauseMenu;
    public UnityEvent stopPauseMenu;

    public void Awake()
    {
        lvlMang = LevelManager.instance; 
    }

    private void Start()
    {
        if (SceneManager.sceneCount >= 1)
        {
            if (SceneManager.GetSceneAt(0).name == playerSceneName)
                StartGame();

            else
                currGameState = gameState.MainMenu;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && currGameState == gameState.Gameplay)
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        currGameState = gameState.Gameplay;
        lvlMang.LoadScenesAdditive(startGameScenes);
    }

    void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        startPauseMenu.Invoke();
    }

    public void UnPauseGame()
    {
        stopPauseMenu.Invoke();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
    }

    public void MainMenu()
    {
        UnPauseGame();
        currGameState = gameState.Loading;
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        lvlMang.SwitchToScene(mainMenuSceneName);
    }

    public void StartGameFromMainMenu()
    {
        currGameState = gameState.Loading;
        lvlMang.SwitchToScene(playerSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
