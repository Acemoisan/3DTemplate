using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Idle State")]
public class IdleState : AIState
{
    [SerializeField] AIState chaseState;
    [SerializeField] AIState patrolState;
    [SerializeField] AIState talkState;
    [SerializeField] AIState followState;
    public override AIState EnemyTick(NPCMovementScript npcNav, NPCCombat enemyCombat)
    {
        // if(enemyNav.GetPlayer() != null)
        // {
        //     return chaseState;
        // }

        return this;
    }

    public override AIState NPCTick(NPCMovementScript npcNav, NPCCombat enemyCombat)
    {
        if(npcNav.GetPlayer() != null) { return chaseState; }

        return this;
    }

}
