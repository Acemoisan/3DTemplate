using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Flee State")]
public class FleeState : AIState
{
    //[SerializeField] AIState chaseState;
    [SerializeField] AIState npcPatrolState;
    [SerializeField] AIState talkState;


    public override AIState EnemyTick(NPCMovementScript npcNav, NPCCombat enemyCombat)
    {
        //if player is in range, change state to chase state

        // if(enemyNav.GetPlayerEntity() != null)
        // {
        //     return chaseState;
        // }

        return this;
    }

    public override AIState NPCTick(NPCMovementScript npcNav, NPCCombat enemyCombat)
    {
        // if(npcNav.GetPlayerEntity() != null) { return talkState; }


        // //npcNav.LookAtPlayer();
        // npcNav.StopNPCMovement();
        return this;
    }

    // public override AIState AnimalTick(HumanoidNavigation npcNav, Animal animal)
    // {
    //     if(npcNav.GetPlayer() == null) return npcPatrolState;

    //     Vector3 _playerPos = npcNav.GetPlayer().transform.position;
        
    //     Vector3 dirToPlayer = npcNav.GetEntity().transform.position - _playerPos;
    //     Vector3 _runAwayPoint = npcNav.GetEntity().transform.position + dirToPlayer;

    //     NPCNavigation nav = (NPCNavigation)npcNav;
    //     npcNav.GetEntity().transform.position = Vector2.MoveTowards(npcNav.GetEntity().transform.position, _runAwayPoint, nav.GetMoveSpeed() * Time.deltaTime);
    //     npcNav.UpdateAnimatorMovement(true);
    //     npcNav.destination = _runAwayPoint;
    //     npcNav.LookAtDestination();
    //     animal.Flee();


    //     // if (npcNav.GetEntity().transform.position == _runAwayPoint)
    //     // {            
    //     //     return npcPatrolState;
    //     // }

    //     if(animal.IsFleeing() == false)
    //     {
    //         return npcPatrolState;
    //     }

    //     return this;
    // }  
}
