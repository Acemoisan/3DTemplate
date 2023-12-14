/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using UnityEngine;
//using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    public LevelEntranceSO levelEntrance;
    public bool activateLoadingScreen;
    public GameObject loadingScreenGO; // Assign in Inspector
    public UnityEngine.UI.Slider loadingBar; // Assign in Inspector if you have a loading bar
    


    [Header("Player Path")]
    public PlayerEntranceSO playerPath;

    //[Header("Broadcasting events")]
    //public LoadSceneRequestGameEvent loadSceneEvent;

    // [Header("ONLY NEEDED FOR MAIN MENU")]
    // [SerializeField] SceneHolderSO sceneSaver;

    public void LoadScene()
    {
        if (this.levelEntrance != null && this.playerPath != null)
        {
            this.playerPath.levelEntrance = this.levelEntrance;
        }

        LoadSceneAsync(sceneToLoad.sceneName);
    }

    public void LoadScene(SceneSO scene)
    {
        if (this.levelEntrance != null && this.playerPath != null)
        {
            this.playerPath.levelEntrance = this.levelEntrance;
        }

        LoadSceneAsync(scene.sceneName);
    }

    private void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {  
        //loadingScreenGO.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Convert loading progress to the range of 0 to 1
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //if (loadingBar != null)
            //{
                //loadingBar.value = progress;
            //}

            Debug.Log("Loading progress: " + operation.progress);
            yield return null; // Wait for the next frame
        }

        Debug.Log("Scene loaded");

        //loadingScreenGO.SetActive(false);
    }
}
