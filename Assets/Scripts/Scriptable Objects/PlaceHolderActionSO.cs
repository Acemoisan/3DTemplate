using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Actions/Placeholder Action")]
public class PlaceHolderActionSO : ItemActionSO
{
    public override void OnPrimaryAction(ItemSO item, PlayerInventory playerInventory, PlayerDamageDealer playerDamageDealer)
    {

    }

    public override void OnItemUsed(ItemSO usedItem, PlayerInventory playerInventory)
    {
        //reduce durability, and other logic here
        if(usedItem.IsStackable())
        {
            playerInventory.RemoveItemFromHotbar(usedItem);
        }
    }
}
