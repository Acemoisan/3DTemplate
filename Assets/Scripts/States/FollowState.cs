using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/Follow State")]
public class FollowState : AIState
{
    [SerializeField] AIState idleState;
    // public override AIState EnemyTick(EnemyCombat enemyCombat, HumanoidNavigation enemyNav)
    // {
    //     return this;
    // }


    // public override AIState NPCTick(HumanoidNavigation enemyNav)
    // {
    //     return this;
    // }

    // public override AIState AnimalTick(HumanoidNavigation nav, Animal animal)
    // {
    //     return this;
    // }  
    // public override AIState SpiritTick(HumanoidNavigation nav) 
    // { 
    //     float dist = Vector2.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

    //     if(dist < 1) { return this; }
        
    //     nav.UpdateAnimatorMovement(true);
    //     nav.GetEntity().transform.position = Vector2.MoveTowards(nav.GetEntity().transform.position, nav.GetPlayer().transform.position, nav.GetMoveSpeed() * Time.deltaTime);

    //     return this; 
    // }  

    // public override AIState CombatRingAnimalTick(HumanoidNavigation nav)
    // {
    //     if(nav.GetPlayer() == null) { return this; }

    //     float dist = Vector2.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

    //     nav.LookAtPlayer();

    //     if(dist > 15)
    //     {
    //         nav.GetEntity().transform.position = nav.GetPlayer().transform.position;
    //         return this;
    //     }

        
    //     if(dist > 1)
    //     {
    //         nav.UpdateAnimatorMovement(true);
    //         nav.GetEntity().transform.position = Vector2.MoveTowards(nav.GetEntity().transform.position, nav.GetPlayer().transform.position, nav.GetMoveSpeed() * Time.deltaTime);
    //         return this;
    //     }


    //     nav.UpdateAnimatorMovement(false);

    //     return this;
    // }
}
