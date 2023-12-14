using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Scriptable Objects/States/NPC Patrol State")]
public class PatrolState : AIState
{
    [SerializeField] AIState fleeState;
    [SerializeField] AIState chaseState;
    [SerializeField] AIState talkState;


    public override AIState NPCTick(NPCMovementScript nav, NPCCombat enemyCombat)
    {
        if(nav.GetPlayer() != null) { return talkState; }

        nav.MoveNPCRandom();
        return this;
    }



    public override AIState EnemyTick(NPCMovementScript nav, NPCCombat enemyCombat)//EnemyCombat enemyCombat, HumanoidNavigation enemyNav)
    {
        if(nav.GetPlayer() != null) { return chaseState; }

        nav.MoveNPCRandom();
        return this; 
    }    

    // public override AIState AnimalTick(HumanoidNavigation nav, Animal animal)
    // {
    //     if(nav.IsStatic()) { return this; }


    //     if(nav.GetPlayer() != null) 
    //     { 
    //         float dist = Vector2.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position);

    //         if(animal.IsWild() == false)
    //         {
    //             if(dist < 2) { return talkState;}
    //         }
    //         else 
    //         {
    //             if(animal.FedTamingFood())
    //             {
    //                 //animal.ResetToWalkSpeed();
    //                 MoveToNavDestination(nav);    
    //             }
    //         }
            


    //         if(animal.GetAnimalClass() == AnimalClass.Passive)
    //         {
    //             if(animal.GetTheFoodTheAnimalEats().Contains(nav.GetPlayer().GetComponent<PlayerInventory>().GetActiveItem()) == true) 
    //             {
    //                 return chaseState;
    //             }
    //             else 
    //             { 
    //                 //animal.ResetToWalkSpeed();
    //                 MoveToNavDestination(nav);               
    //             }      
    //         }   
    //         else if(animal.GetAnimalClass() == AnimalClass.Aggressive)
    //         {
    //             return chaseState;
    //         }
    //     }
    //     else 
    //     {
    //         MoveToNavDestination(nav);
    //     }

    //     return this;
    // }

    // AIState MoveToNavDestination(HumanoidNavigation nav)
    // {
    //     nav.GetEntity().transform.position = Vector2.MoveTowards(nav.GetEntity().transform.position, nav.GetDestination(), nav.GetMoveSpeed() * Time.deltaTime);
    //     nav.UpdateAnimatorMovement(true);
    //     nav.LookAtDestination();

    //     if(nav.ReachedDestination())
    //     {
    //         if(nav.WaitingInPlace())
    //         {
    //             return this;
    //         }

    //         nav.SetNewStopTimer();
    //         nav.SetNewDestination();

    //         return this;
    //     }

    //     return this;
    // }


}
