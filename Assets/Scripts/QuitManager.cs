/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using UnityEditor;
using UnityEngine;


public class QuitManager : MonoBehaviour
{
    public void QuitGame()
    {
        Invoke("QuitDelay", 0.5f);
    }

    void QuitDelay()
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}