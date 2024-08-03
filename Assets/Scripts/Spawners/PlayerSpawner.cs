/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerEntranceSO playerPath;
    public UnityEngine.GameObject playerPrefab;
    public LevelEntrance defaultEntrance;

    public void InstantiatePlayerOnLevel()
    {
        UnityEngine.GameObject player = GetPlayer();

        Transform entrance = GetLevelEntrance(playerPath.levelEntrance);

        player.transform.position = entrance.position;
    }

    public UnityEngine.GameObject GetPlayer()
    {
        UnityEngine.GameObject playerObject;

        if(UnityEngine.GameObject.FindGameObjectWithTag("Player") == null)
        {
            playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);//, playerParent.transform);
        }
        else 
        {
            playerObject = UnityEngine.GameObject.FindGameObjectWithTag("Player");
        }

           return playerObject;
    }

    private Transform GetLevelEntrance(LevelEntranceSO playerEntrance)
    {

        var levelEntrances = UnityEngine.GameObject.FindObjectsOfType<LevelEntrance>();

        foreach (LevelEntrance levelEntrance in levelEntrances)
        {
            if (levelEntrance.entrance == playerEntrance)
            {
                return levelEntrance.gameObject.transform;
            }
        }

        return defaultEntrance.gameObject.transform;
    }
}
