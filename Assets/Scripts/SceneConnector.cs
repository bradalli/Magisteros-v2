using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConnector : MonoBehaviour
{
    private LevelManager lvlMang;
    private Scene myScene;

    [SerializeField] string[] aScenesNames;
    [SerializeField] string[] bScenesNames;

    [SerializeField] bool aEntered = false;
    [SerializeField] bool bEntered = false;

    private void Awake()
    {
        lvlMang = LevelManager.instance;
    }

    public void TriggerAEntered()
    {
        Debug.Log("A ENTERED");

        // Walking in from A side
        if (!aEntered && !bEntered)
        {
            lvlMang.LoadScenesAdditive(bScenesNames);
        }

        // Returning to A side without entering B side
        if(aEntered && !bEntered)
        {
            lvlMang.UnloadScenes(bScenesNames);
        }

        // Returning to A side with having partially entered B side
        if(aEntered && bEntered)
        {
            lvlMang.UnloadScenes(bScenesNames);
            lvlMang.LoadScenesAdditive(aScenesNames);
        }

        // Walking in from B side
        if (!aEntered && bEntered)
        {
            lvlMang.UnloadScenes(bScenesNames);
            lvlMang.LoadScenesAdditive(aScenesNames);
        }

        aEntered = !aEntered;

        if (bEntered && aEntered)
        {
            bEntered = false;
            aEntered = false;
        }
    }

    public void TriggerBEntered()
    {
        Debug.Log("B ENTERED");

        // Walking in from B side
        if (!bEntered && !aEntered)
        {
            lvlMang.LoadScenesAdditive(aScenesNames);
        }

        // Returning to B side without entering A side
        if(bEntered && !aEntered)
        {
            lvlMang.UnloadScenes(aScenesNames);
        }

        // Returning to B side with having partially entered A side
        if(bEntered && aEntered)
        {
            lvlMang.UnloadScenes(aScenesNames);
            lvlMang.LoadScenesAdditive(bScenesNames);
        }

        // Walking in from A side
        if (!bEntered && aEntered)
        {
            lvlMang.UnloadScenes(aScenesNames);
            lvlMang.LoadScenesAdditive(bScenesNames);
        }

        bEntered = !bEntered;

        if(bEntered && aEntered)
        {
            bEntered = false;
            aEntered = false;
        }
    }
}
