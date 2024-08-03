/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using UnityEngine;
using System.Collections.Generic;
using ScriptableObjectArchitecture;

[System.Serializable]
public class InventoryConsumable
{
    public ConsumableSO item;
    public int amount;

    public InventoryConsumable(ConsumableSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}


[System.Serializable] 
public class ItemSlot
{
    //public int ID;
    public ItemSO item;
    public int count;


    //referenced in Drag and Drop
    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Set(ItemSO item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}
public class Inventory
{
}