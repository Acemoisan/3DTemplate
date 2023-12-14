using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Attack State")]
public class AttackState : AIState
{
    [SerializeField] AIState chaseState;
    public override AIState EnemyTick(NPCMovementScript nav, NPCCombat enemyCombat)
    {
        if(nav.GetPlayer() == null) { return chaseState;}

        float dist = Vector3.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

        
        if(enemyCombat.IsCloseEnoughForBasicAttack() == false) 
        { FollowPlayer(nav); }
        else 
        { nav.SetDestination(nav.GetEntity().transform.position); }
        //FollowPlayer(nav);
        //Debug.Log("Attack State: " + dist);

        if(dist > enemyCombat.GetAttackAlertRange())
        {
            return chaseState;
        }
        else
        {
            
            if(enemyCombat.IsAttacking()) 
            { 
                return this; 
            } 
            else 
            { 
                enemyCombat.StartAttack(); 
            }

            // if(enemyCombat.IsCloseEnoughForBasicAttack()) { return this; }
            // else FollowPlayer(nav);
        }

        return this;
    }



    public override AIState NPCTick(NPCMovementScript nav, NPCCombat enemyCombat)
    {
        return this;
    }
 

    void FollowPlayer(NPCMovementScript nav)
    {
        nav.SetDestination(nav.GetPlayer().transform.position);
    }   
}
