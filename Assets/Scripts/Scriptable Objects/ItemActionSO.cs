using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionSO : ScriptableObject
{
    public virtual void OnPrimaryAction(ItemSO item, PlayerInventory playerInventory, PlayerDamageDealer playerDamageDealer)
    {
        Debug.Log("On Apply was not usd as an override:");
    }

    public virtual void OnItemUsed(ItemSO usedItem, PlayerInventory playerInventory)
    {
        Debug.Log("On Apply was not usd as an override:");
    }
}
