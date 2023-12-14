/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Consumable")]

public class ConsumableSO : ItemSO
{
    [Header("<color=#ffffff>XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX</color>")]
    [Header("<color=#19bfbf>XXXX CONSUMABLE XXXXX CONSUMABLE XXXX</color>")]
    [Space(50)]


    [Range(0, 500)] public float energyRegainValue;
    [Range(0, 500)] public float manaRegainValue;
    [Range(0, 1000)] public float healthRegainValue;
    //public ToolAction onPotionAction;
}
