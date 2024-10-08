/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using ScriptableObjectArchitecture;
using UnityEngine;
//using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    //public LevelEntranceSO levelEntrance;
    public bool showLoadingScreen;
    


    // [Header("Player Path")]
    // public PlayerEntranceSO playerPath;




    public void LoadScene()
    {
        if(SceneLoaderManager.Instance == null)
        {
            Debug.LogError("SceneLoaderManager is not initialized");
            return;
        }
        if(sceneToLoad == null)
        {
            Debug.LogError("SceneSO is not set");
            return;
        }
        SceneLoaderManager.Instance.OnLoadLevelRequest(sceneToLoad, showLoadingScreen);
    }

    public void LoadScene(SceneSO scene)
    {
        if(SceneLoaderManager.Instance == null)
        {
            Debug.LogError("SceneLoaderManager is not initialized");
            return;
        }

        SceneLoaderManager.Instance.OnLoadLevelRequest(scene, showLoadingScreen);
    }

    public void ReloadScene()
    {
        if(SceneLoaderManager.Instance == null)
        {
            Debug.LogError("SceneLoaderManager is not initialized");
            return;
        }
        SceneLoaderManager.Instance.ReloadCurrentScene();
    }
}
