using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState : AIStateManager
{
    [SerializeField] NPCMovementScript nav;
    [SerializeField] NPCCombat combat;

    public override void HandleCurrentState()
    {
        if (currentState != null)
        {
            AIState nextState = currentState.NPCTick(nav, combat);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    void SwitchToNextState(AIState state)
    {
        currentState = state;
    }
}
