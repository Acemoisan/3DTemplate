using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState: ScriptableObject
{
    public virtual AIState EnemyTick(NPCMovementScript movement, NPCCombat enemyCombat) { Debug.Log("This is the base class"); return null; }
    public virtual AIState NPCTick(NPCMovementScript movement, NPCCombat enemyCombat) { Debug.Log("This is the base class"); return null;}

}
