/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTool", menuName = "Scriptable Objects/Item/Tool")]
public class ItemToolSO : ItemSO
{
[Header("To see visible tool range. Add Draw Gizmo script and reference a side of the player")]
    [SerializeField] float energyDepletionValue;
    [SerializeField] float toolRange;



    public float GetEnergyDepletionLevel()
    {
        return energyDepletionValue;
    }

    public float GetToolRange()
    {
        return toolRange;
    }



    public override float GetAttackSpeedMultiplier()
    {
        return 0;
    }
}
