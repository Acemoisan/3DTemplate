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

    public void SetupDamageDealer(ItemSO item, DamageClasses damageType)//this is setup prior to animation playing. so that the collider has the right information
    {
        currentDamageType = damageType;
        damage = item.GetToolDamage();
    }

    public override void PerformDamage()
    {
        //if the damageable is a resource node, then check if the damageable is the same type as the damage type
        if(damageableTargetRef != null && damageableTargetRef as ResourceNodeCollider != null)
        {
            Debug.Log("Damageable is a resource node");
            ResourceNodeCollider resourceNode = damageableTargetRef as ResourceNodeCollider;
            if(resourceNode.bestHarvestedWith == currentDamageType)
            {
                Debug.Log("Damage type matches resource type"); //APPLY BONUS RESOURCES + DAMAGE
            }
            base.PerformDamage();
        }
        else
        {
            base.PerformDamage();
        }

        base.PerformDamage();
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    public void SetupFists(float fistDamage)
    {
        currentDamageType = DamageClasses.Fists;
        SetDamage(fistDamage);
    }

    public void SetDamageType(DamageClasses damageType)
    {
        currentDamageType = damageType;
    }
}
