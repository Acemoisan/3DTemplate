using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneStoneCollider : MonoBehaviour, Damageable
{
    [Header("Runestone Attributes")]
    [SerializeField] UnityEngine.GameObject runeStoneObject;
    public ProjectileType runeType;
    [SerializeField] float runeHealth;
    public UnityEvent OnHit;
    public UnityEvent OnDestroy;

    
    public void Hit(float damage)
    {
        runeHealth -= damage;
        OnHit?.Invoke();
        if(runeHealth <= 0)
        {
            if(OnDestroy != null) OnDestroy.Invoke();
            Destroy(runeStoneObject);
        }
    }

}
