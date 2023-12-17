using UnityEngine;

public class Bullet : Damage
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject attackVFX;
    Vector3 shootingDirection;
    [SerializeField] float destroyTimer = 5;
    bool readyToMove = false;

    protected virtual void Start()
    {
        SetupProjectile();
    }

    public override void PerformDamage()
    {
        base.PerformDamage();
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }


    private void HandleCollision(GameObject otherObject)
    {
        //collision is handled in the Base.Damage script. By checking for OnTriggerEnter
    }

    public void SetupProjectile()
    {
        Destroy(this.gameObject, destroyTimer);
        rb.velocity = transform.forward * projectileSO.projectileSpeed;
    }

    public void SetDirection(Vector3 direction)
    {
        shootingDirection = direction.normalized;
        readyToMove = true;
    }

    public void InstantiateAttackVFX()
    {
        if(attackVFX == null) return;

        GameObject vfx = Instantiate(attackVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(vfx, 1f);
    }
}
