using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : Damage
{

    public override void PerformDamage()
    {
        base.PerformDamage();
    }
    public override void OnDamage()
    {
        Debug.Log("DeathBarrier");
    }
}
