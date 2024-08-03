using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCAnimationEvents : MonoBehaviour
{
    [SerializeField] NPCCombat nPCCombat;
    public UnityEvent MovementOn;
    public UnityEvent MovementOff;
    public UnityEvent EnableColliderEvent;
    public UnityEvent DisableColliderEvent;


    public void AttackHitEvent(UnityEngine.GameObject obj)
    {
        nPCCombat.InstantiateAttackEffect(obj);
    }


    public void TriggerMovementOn()
    {
        MovementOn.Invoke();
    }

    public void TriggerMovementOff()
    {
        MovementOff.Invoke();
    }

    public void EnableCollider()
    {
        EnableColliderEvent.Invoke();
    }

    public void DisableCollider()
    {
        DisableColliderEvent.Invoke();
    }
}
