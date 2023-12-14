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
    [Header("XXXX TOOL XXXXX TOOL XXXXX TOOL XXXX")]
    [Space(50)]



    //[SerializeField] bool worldTool; //Used by tool controller 
    //[SerializeField] bool gridTool; //Used by tool controller
    //public List<ToolAction> primaryToolActions;
    //public List<ToolAction> secondaryToolActions;
    //public ToolAction onWorldAction; 
    //public ToolAction onTileMapAction;


    [Header("To see visible tool range. Add Draw Gizmo script and reference a side of the player")]
    [SerializeField] float energyDepletionValue;
    [SerializeField] float primaryToolRange;
    [SerializeField] float secondaryToolRange; 
    [SerializeField] float primaryToolDamage;
    [SerializeField] float secondaryToolDamage;


    //[Header("Animation Trigger")]
    //public AnimationSO chargeUpAnimationTriggerName;


    [Header("Marker")]
    [SerializeField] bool thisToolMarksGroundWithTile;

    public float GetEnergyDepletionLevel()
    {
        return energyDepletionValue;
    }

    public float GetPrimaryToolRange()
    {
        return primaryToolRange;
    }

    public float GetSecondaryToolRange()
    {
        return secondaryToolRange;
    }

    public float GetPrimaryToolDamage()
    {
        return primaryToolDamage;
    }

    public float GetSecondaryToolDamage()
    {
        return secondaryToolDamage;
    }

    public override float GetAttackSpeed()
    {
        return 0;
    }

    public bool ThisToolMarksGroundWithTile()
    {
        return thisToolMarksGroundWithTile;
    }
}
