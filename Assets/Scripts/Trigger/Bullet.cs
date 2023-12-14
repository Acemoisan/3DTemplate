using UnityEngine;

public class Bullet : Damage
{

    [SerializeField] protected float speed = 10f;
    [SerializeField] GameObject attackVFX;
    Vector3 shootingDirection;
    [SerializeField] float destroyTimer = 5;
    bool readyToMove = false;

    protected virtual void Start()
    {
        Destroy(this.gameObject, destroyTimer);
    }

    public override void PerformDamage()
    {
        base.PerformDamage();
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    
    private void Update()
    {
        if(readyToMove == false) { return; }
        transform.Translate(shootingDirection * speed * Time.deltaTime, Space.World);
        Debug.Log(shootingDirection);
        //transform.position =  Vector3.forward * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        //shootingDirection = (direction - transform.position).normalized;

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
