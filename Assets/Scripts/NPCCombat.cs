using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : MonoBehaviour
{
    [SerializeField] NPCMovementScript nav;
    [SerializeField] NPCAnimation npcAnim;
    [SerializeField] NPCTrigger alertTriggerChecker;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> attackSFX;
    [SerializeField] List<GameObject> playerTargetList;
    //[SerializeField] LayerMask playerLayerMask;
    [SerializeField] string attackOneAnimationString;
    [SerializeField] string attackTwoAnimationString;
    [SerializeField] Transform attackPoint;
    [Range(0, 5)] [SerializeField] float minDelayBetweenAttacks;
    [Range(0, 5)] [SerializeField] float maxDelayBetweenAttacks;
    [Range(0, 100)] [SerializeField] float chanceOfSpecialAttack;
    [Range(0, 100)] [SerializeField] float attackAlertRange;
    public float GetAttackAlertRange() { return attackAlertRange; }
    [Range(0, 100)] [SerializeField] float basicAttackRange;
    [SerializeField] AttackAction specialAttack;
    [SerializeField] bool canBasicAttack;



    float randomAttackRate;
    bool isAttacking = false;





    #region ATTACKING ===============

    public void StartAttack()
    {
        isAttacking = true;
        StartCoroutine(AttackDelay());
    }


    IEnumerator AttackDelay()
    {
        //OnAttack?.Invoke();
        PerformAttack();
        yield return new WaitForSeconds(Random.Range(minDelayBetweenAttacks, maxDelayBetweenAttacks));
        isAttacking = false;
    }


    public void PerformAttack()
    {
        if (IsCloseEnoughForBasicAttack())
        {
            if(Random.Range(1, 100) <= chanceOfSpecialAttack)
            {
                SpecialAttack();
            }
            else 
            {
                BasicAttack();
            }
        }
        else 
        {
            SpecialAttack();
        }

        PlayRandomHitSFX();
    }


    public bool IsAttacking()
    {
        return isAttacking;
    }


    public bool IsCloseEnoughForBasicAttack()
    {        
        if (Vector3.Distance(nav.GetEntity().transform.position, nav.GetPlayer().transform.position) < basicAttackRange)
        {
            canBasicAttack = true;
            return true;
        }
        else 
        {
            canBasicAttack = false;
            return false;
        }
    }


    void BasicAttack()
    {
        npcAnim.TriggerAnimation(attackOneAnimationString);

        // if(Random.Range(0, 2) == 0)
        // {
        //     npcAnim.TriggerAnimation(attackOneAnimationString);
        // }
        // else
        // {
        //     npcAnim.TriggerAnimation(attackTwoAnimationString);
        // }


        // To Do - THIS WILL LATER BE A BOX COLLIDER TURNED ON BY ATTACK ANIM
        // Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyNavigation.GetEntity().transform.position, enemySO.GetBasicAttackRange(), playerLayerMask);

        // foreach (Collider2D col in colliders)
        // {
        //     IDamageable hit = col.GetComponentInParent<IDamageable>();
        //     if (hit != null)
        //     {     
        //         hit.Hit(enemySO.GetDamage());
        //     } 
        // }
    }

    void SpecialAttack()
    {
        //specialAttack.OnAttack(nav.GetEntity(), nav.GetEntity().transform.position, nav.GetPlayer().transform.position);
        npcAnim.TriggerAnimation(attackTwoAnimationString);
    }

    public void InstantiateAttackEffect(GameObject vfx)
    {
        Instantiate(vfx, attackPoint.position, attackPoint.rotation);
    }

    #endregion



    public void AddPlayerToTargetList()
    {
        if (playerTargetList.Contains(alertTriggerChecker.GetPlayer())) { return; }
        playerTargetList.Add(alertTriggerChecker.GetPlayer());
    }

    public void RemovePlayerFromTargetList()
    {
        foreach (GameObject player in playerTargetList)
        {
            if (player == alertTriggerChecker.GetPlayer())
            {
                playerTargetList.Remove(player);
            }
        }
    }

    GameObject FindRandomTarget()
    {
        return playerTargetList[Random.Range(0, playerTargetList.Count)];
    }

    public void PlayRandomHitSFX()
    {
        if(Random.Range(0, 100) < 50)
        {
            audioSource.PlayOneShot(attackSFX[Random.Range(0, attackSFX.Count)]);
        }
    }
}
