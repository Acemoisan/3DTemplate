using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Enemy Chase State")]
public class ChaseState : AIState
{
    [SerializeField] AIState attackState;
    [SerializeField] AIState patrolState;



    public override AIState EnemyTick(NPCMovementScript nav, NPCCombat enemyCombat)
    {
        if(nav.GetPlayer() == null) { return patrolState;}

        float dist = Vector3.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

        FollowPlayer(nav);
        if(dist < enemyCombat.GetAttackAlertRange())
        {
            return attackState;
        }

        return this;
    }


    public override AIState NPCTick(NPCMovementScript nav, NPCCombat enemyCombat)
    {
        if(nav.GetPlayer() == null) { return patrolState;}

        float dist = Vector3.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

        FollowPlayer(nav);
        if(dist < enemyCombat.GetAttackAlertRange())
        {
            return attackState;
        }

        return this;
        
    }


    void FollowPlayer(NPCMovementScript nav)
    {
        nav.SetDestination(nav.GetPlayer().transform.position);
        //nav.MoveNPCRandom();
        //nav.GetEntity().transform.position = Vector2.MoveTowards(nav.GetEntity().transform.position, nav.GetPlayer().transform.position, nav.GetMoveSpeed() * Time.deltaTime);
        //nav.LookAtPlayer();
        //nav.UpdateAnimatorMovement(true); 
    }
}
