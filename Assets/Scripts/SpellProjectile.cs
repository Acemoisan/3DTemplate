using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileType
{
    Fire,
    Light,
    Electricity,
}
public class SpellProjectile : Damage
{
    [SerializeField] ProjectileType projectileType;
    [SerializeField] protected float speed = 10f;
    [SerializeField] UnityEngine.GameObject attackVFX;
    Vector3 shootingDirection;
    [SerializeField] float destroyTimer = 5;

    protected virtual void Start()
    {
        Destroy(this.gameObject, destroyTimer);
    }

    public override void PerformDamage()
    {
        if(damageableTargetRef as RuneStoneCollider != null)
        {
            RuneStoneCollider runeStone = damageableTargetRef as RuneStoneCollider;
            if(runeStone.runeType == projectileType)
            {
                base.PerformDamage();
            }
        }
        else
        {
            base.PerformDamage();
        }
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    
    private void Update()
    {
        transform.Translate(shootingDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        //Vector3 shootingDir = (direction - transform.position).normalized;

        shootingDirection = direction;
    }

    public void InstantiateAttackVFX()
    {
        if(attackVFX == null) return;

        UnityEngine.GameObject vfx = Instantiate(attackVFX, transform.position, Quaternion.identity) as UnityEngine.GameObject;
        Destroy(vfx, 1f);
    }
}
