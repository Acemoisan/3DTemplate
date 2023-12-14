using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour, Damageable
{
    [SerializeField] PlayerAttributes playerHealth;



    public void Hit(float damage)
    {
        playerHealth.DecreaseHealth(damage);
    }
}
