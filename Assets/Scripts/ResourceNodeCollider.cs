using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeCollider : MonoBehaviour, Damageable
{
    [SerializeField] GameObject node;
    [SerializeField] float maxhealth;
    public DamageClasses bestHarvestedWith;
    float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
    }


    public void Hit(float damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //visualize damage to resource

        //OnHit?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        currentHealth = 0;
        //OnDeath?.Invoke();
        Destroy(node, .25f);
    }
}
