using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    public bool showLoadingScreen;
    


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
