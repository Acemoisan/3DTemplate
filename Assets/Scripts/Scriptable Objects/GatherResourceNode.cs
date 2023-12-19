using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Actions/Gather Resource Node Action")]
public class GatherResourceNode : ItemActionSO
{
    public override void OnPrimaryAction(ItemSO item, PlayerInventory playerInventory)
    {
    }

    public override void OnItemUsed(ItemSO usedItem, PlayerInventory playerInventory)
    {
        //reduce durability, and other logic here
        playerInventory.RemoveItemFromHotbar(usedItem);
    }
}
