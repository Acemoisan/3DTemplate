using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Action/Attack/Projectile")]
public class ProjectileAttack : AttackAction
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;

    

    public override void OnAttack(GameObject entity, Vector2 position, Vector2 attackDestination)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        Vector2 direction = (attackDestination - position).normalized;
        projectile.GetComponent<Projectile>().SetUp(direction, projectileSpeed, spellDuration);
    }
}

