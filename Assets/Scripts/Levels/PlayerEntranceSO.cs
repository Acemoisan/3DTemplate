/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The level entrance that the player should use to spawn at. ie. if the player entered the beach from the town (to the right of the beach) then this will contain BeachEntranceFromRight so that the player spawns on the right side of the beach.
/// </summary>
[CreateAssetMenu(fileName = "PlayerPath", menuName = "Scriptable Objects/Level/Player Path")]
public class PlayerEntranceSO : ScriptableObject
{
    [Header("Scene Information")]
    public LevelEntranceSO levelEntrance;
}
