using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Damage : MonoBehaviour
{
    [SerializeField] protected ProjectileSO projectileSO;
    [SerializeField] protected bool damageOverTime;



    [Header("Random Damage")]
    [SerializeField] protected bool randomDamage;
    [Range(0, 1000)] [SerializeField] protected float minimumDamage;
    [Range(0, 1000)] [SerializeField] protected float maximumDamage;

    [Header("Events")]
    public UnityEvent OnDamageEvent;

    Collider target;
    protected Damageable damageableTargetRef;



    
    void OnTriggerEnter(Collider other)
    {
        OnDamage();

        if(other.GetComponent<Damageable>() == null) return;
        Debug.Log(other.name);
        target = other;
        damageableTargetRef = target.GetComponent<Damageable>();

        //if damageable as runeCollider != null 
        //then ask for rune collidertype. 
        //if type matches, do damage. return regardless

        PerformDamage();
    }

    public virtual void PerformDamage()
    {
        if(damageOverTime) {
            StartCoroutine(DamageOverTime());
        }
        else {
            if(target != null)
            {
                target.GetComponent<Damageable>().Hit(GetDamage());
            }
        }
    }

    public virtual void OnDamage()
    {
        if(OnDamageEvent != null) OnDamageEvent.Invoke();
    }
    
    IEnumerator DamageOverTime()
    {
        Damageable damageable = target.GetComponent<Damageable>();
        while(target != null)
        {
            damageable.Hit(GetDamage());
            yield return new WaitForSeconds(1f);
        }
    }

    float GetDamage()
    {
        if(randomDamage) {
            return Random.Range(minimumDamage, maximumDamage);
        }
        else {
            return projectileSO.projectileDamage;
        }
    }

    void OnDestroy()
    {
        StopCoroutine(DamageOverTime());
    }
}
