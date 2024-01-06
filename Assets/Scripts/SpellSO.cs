using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "NewSpell", menuName = "Scriptable Objects/Item/Spell")]
public class SpellSO : ItemSO
{
    public SpellType spellType;
    public GameObject spellPrefab;
}


[System.Serializable]
public enum SpellType
{
    Projectile,
    Aura,
    SpeedBoost
}
