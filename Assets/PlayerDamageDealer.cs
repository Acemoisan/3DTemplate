using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageClasses
{
    Fists,
    Sword,
    Pickaxe,
    Axe,
    Spell
}

public class PlayerDamageDealer : Damage
{
    [SerializeField] DamageClasses currentDamageType;
    //[SerializeField] PlayerInventory playerInventory;

    public void SetupDamageDealer(ItemSO item, DamageClasses damageType)
    {
        currentDamageType = damageType;
        damage = item.GetToolDamage();
    }

    public override void PerformDamage()
    {
        float chanceOfGettingresources;
        //if the damageable is a resource node, then check if the damageable is the same type as the damage type
        if(damageableTargetRef != null && damageableTargetRef as ResourceNodeCollider != null)
        {
            Debug.Log("Damageable is a resource node");
            ResourceNodeCollider resourceNode = damageableTargetRef as ResourceNodeCollider;
            if(resourceNode.bestHarvestedWith == currentDamageType)
            {
                Debug.Log("Damage type matches resource type"); //APPLY BONUS RESOURCES + DAMAGE
                chanceOfGettingresources = Random.Range(0, 80); //Generally an 80% Chance of Getting resources.
            }
            else 
            {
                chanceOfGettingresources = Random.Range(0, 20); //Generally a 20% Chance of Getting resources.
            }


            //Give Resources To Player OR Drop them on ground.
            resourceNode.SpawnItem(chanceOfGettingresources);
            base.PerformDamage();
        }
        else
        {
            base.PerformDamage();
        }
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    public void SetupFists(float fistDamage)
    {
        SetDamageType(DamageClasses.Fists);
        SetDamage(fistDamage);
    }

    public void SetDamageType(DamageClasses damageType)
    {
        currentDamageType = damageType;
    }
}
