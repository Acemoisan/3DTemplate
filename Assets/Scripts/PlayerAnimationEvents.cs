using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMagic playerMagic;


    public void AttackHitEvent(GameObject vfx)
    {
        //playerCombat.InstantiateAttackEffectAtHand(vfx);
    }

    public void SpawnMagicAuraEvent()
    {
        playerMagic.SpawnMagicAura();
    }

    public void LeftHandSpellEvent()
    {
        playerMagic.SpawnProjectileLeftHand();
    }

    public void RightHandSpellEvent()
    {
        playerMagic.SpawnProjectileRightHand();
    }
}
