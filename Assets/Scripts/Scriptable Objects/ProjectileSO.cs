using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manager", menuName = "Scriptable Objects/Projectile SO")]
public class ProjectileSO : ScriptableObject
{
    public float projectileDamage;
    public float projectileSpeed;
}
