using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCAttributes : MonoBehaviour
{
    [SerializeField] HealthBarManager healthBar;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> hitSFX;

    [Header("Events")]
    public UnityEvent OnHit;
    public UnityEvent OnDeath;
    public UnityEvent OnRevive;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UpdateHealthBar();

        if(OnHit != null) OnHit.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        currentHealth = 0;
        if(OnDeath != null) OnDeath.Invoke();
        Destroy(this.gameObject.transform.parent.gameObject, 5);
    }

    public void Revive()
    {
        currentHealth = maxHealth;
        if(OnRevive != null) OnRevive.Invoke();
    }

    private void UpdateHealthBar()
    {
        if(healthBar == null) { Debug.LogError("You must assign a healthBar to " + this); return; }
        healthBar.UpdateHealthBars(currentHealth, maxHealth);
    }

    public void PlayRandomHitSFX()
    {
        if(Random.Range(0, 100) < 30)
        {
            audioSource.PlayOneShot(hitSFX[Random.Range(0, hitSFX.Count)]);
        }
    }
}
