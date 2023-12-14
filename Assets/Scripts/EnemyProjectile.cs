using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        SetDirection(Vector3.forward);
        base.Start();
    }
}
