using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionSO : ScriptableObject
{
    public virtual void OnPrimaryAction(ItemSO item)
    {
        Debug.Log("On Apply was not usd as an override:");
    }
}
