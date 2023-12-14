using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCAnimationEvents : MonoBehaviour
{
    [SerializeField] NPCCombat nPCCombat;
    public UnityEvent MovementOn;
    public UnityEvent MovementOff;

    public void AttackHitEvent(GameObject obj)
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
}
