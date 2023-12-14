using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ScriptableObject
{
    //public bool stopMovement;
    //public float chargeUpSpeed;
    public float spellDuration;

    public virtual void OnAttack(GameObject entity, Vector2 position, Vector2 attackDestination)//Vector2 worldPoint, float toolRange, float toolDamage) //this class will be over rode in daughter classes
    {
        Debug.Log("OnAttack was not used as an override");
    }
}
