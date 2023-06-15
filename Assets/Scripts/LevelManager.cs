using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] bool loading, unloading;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScenesAdditive(string[] scenes)
    {
        if (!loading)
            StartCoroutine(LoadingScenesAdditive(scenes));
    }

    public void UnloadScenes(string[] scenes)
    {
        if(!unloading)
            StartCoroutine(UnloadingScenes(scenes));
    }

    IEnumerator LoadingScenesAdditive(string[] scenes)
    {
        loading = true;

        List<string> loadedSceneNames = new List<string>();
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
        }

        foreach(string scene in scenes)
        {
            if (!loadedSceneNames.Contains(scene))
            {
                AsyncOperation loadingState = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadingState.isDone);
            }
        }

        loading = false;
    }

    IEnumerator UnloadingScenes(string[] scenes)
    {
        unloading = true;

        List<string> loadedSceneNames = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
        }

        foreach (string scene in scenes)
        {
            if (loadedSceneNames.Contains(scene))
            {
                AsyncOperation unloadingState = SceneManager.UnloadSceneAsync(scene);
                yield return new WaitUntil(() => unloadingState.isDone);
            }

            else
                yield return null;
        }

        unloading = false;
    }
}
