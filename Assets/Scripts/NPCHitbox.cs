using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitbox : MonoBehaviour, Damageable
{
    [SerializeField] NPCAttributes npcAttributes;



    public void Hit(float damage)
    {
        npcAttributes.TakeDamage(damage);
    }
}
