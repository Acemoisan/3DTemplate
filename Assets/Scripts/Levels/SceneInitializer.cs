/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [Header("Dependencies")]
    public SceneSO[] sceneDependencies;

    [Header("On Scene Ready")]
    public UnityEvent onDependenciesLoaded;




    public void Start()
    {
        StartCoroutine(LoadDependencies());
    }

    private IEnumerator LoadDependencies()
    {
        for (int i = 0; i <= this.sceneDependencies.Length - 1; ++i)
        {
            SceneSO sceneToLoad = this.sceneDependencies[i];

            if (!SceneManager.GetSceneByName(sceneToLoad.name).isLoaded)
            {
                Debug.Log("Loading scene: " + sceneToLoad.name);
                var loadOperation = SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Additive);

                while (!loadOperation.isDone)
                {
                    yield return null;
                }
            }
        }

        if (onDependenciesLoaded != null)
        {
            onDependenciesLoaded.Invoke();
        }
    }
}
