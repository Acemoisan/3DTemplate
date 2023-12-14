using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Talk State")]
public class TalkState : AIState
{
    [SerializeField] AIState chaseState;
    [SerializeField] AIState patrolState;
    [SerializeField] float dampingSpeed;
    
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
        if(npcNav.GetPlayer() != null) 
        { 
            npcNav.GetNavMeshAgent().SetDestination(npcNav.GetEntity().transform.position);
            Vector3 direction = npcNav.GetPlayer().transform.position - npcNav.GetEntity().transform.position;

            // Calculate the rotation needed to point towards the target
            Quaternion toRotation = Quaternion.LookRotation(direction);

            // Lerp the object's rotation towards the calculated rotation
            npcNav.GetEntity().transform.rotation = Quaternion.Slerp(npcNav.GetEntity().transform.rotation, toRotation, Time.deltaTime * dampingSpeed);            
            return this;
        }
        else return patrolState;
    }

}
