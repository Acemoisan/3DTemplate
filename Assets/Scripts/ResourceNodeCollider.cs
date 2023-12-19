using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.TerrainUtils;

[RequireComponent(typeof(SpawnItem))]
[RequireComponent(typeof(Collider))]
public class ResourceNodeCollider : MonoBehaviour, Damageable
{
    [SerializeField] GameObject node;
    [SerializeField] float maxhealth;
    public DamageClasses bestHarvestedWith;
    [SerializeField] SpawnItem spawnItem;
    [ReadOnly]
    [SerializeField] float currentHealth;

    public UnityEvent OnHit;
    public UnityEvent OnDeath;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
        InvokeRepeating("Test", 1f, 1f);
    }

    void Test()
    {
        Debug.Log("Test");
    }


    public void Hit(float damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //visualize damage to resource

        OnHit?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SpawnItem(float dropChance)
    {
        if(spawnItem == null) { Debug.LogError(this + " has no spawn item Ref"); return;}
        spawnItem.SetDropChance(dropChance);
        spawnItem.SpawnAnItem();
    }

    public void Die()
    {
        currentHealth = 0;
        OnDeath?.Invoke();
        Destroy(node, .25f);
    }
}
