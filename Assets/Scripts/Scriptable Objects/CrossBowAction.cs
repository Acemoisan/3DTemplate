using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Actions/Shoot Bow Action")]
public class CrossBowAction : ItemActionSO
{
    //[SerializeField] DamageClasses damageType; //IF I NEED TO, I CAN USE THIS DAMAGE TYPE. INSTEAD OF HAVING IT ON THE ITEM ITSELF.

    public override void OnPrimaryAction(ItemSO item, PlayerInventory playerInventory, PlayerDamageDealer playerDamageDealer)
    {
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
        AudioManager.instance.PlaySound(Sound.CrossBow);
    }

}
