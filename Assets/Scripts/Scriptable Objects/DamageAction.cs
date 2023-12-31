using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Actions/Gather Resource Node Action")]
public class DamageAction : ItemActionSO
{
    //[SerializeField] DamageClasses damageType; //IF I NEED TO, I CAN USE THIS DAMAGE TYPE. INSTEAD OF HAVING IT ON THE ITEM ITSELF.

    public override void OnPrimaryAction(ItemSO item, PlayerInventory playerInventory, PlayerDamageDealer playerDamageDealer)
    {
        DamageClasses damageType = item.GetDamageClass();
        playerDamageDealer.SetupDamageDealer(item, damageType);
        OnItemSound();
    }

    public override void OnItemUsed(ItemSO usedItem, PlayerInventory playerInventory)
    {
        //reduce durability, and other logic here
        if(usedItem.IsStackable())
        {
            playerInventory.RemoveItemFromHotbar(usedItem);
        }
    }

    public override void OnItemSound()
    {
        AudioManager.instance.PlaySound(Sound.SwingTool);
    }

}
