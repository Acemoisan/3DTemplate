using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Damage
{
    Vector2 direction;
    float speed;
    float spellDuration;

    public override void OnDamage()
    {
        Debug.Log("Projectile");
    }

    public void SetUp(Vector2 direction, float speed, float spellDuration)
    {
        this.direction = direction;
        this.speed = speed;
        this.spellDuration = spellDuration;
        StartCoroutine(DestroyAfterTime(spellDuration));

        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(direction));
    }

    float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}
