using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform handPositionAttackPoint;
    [SerializeField] Camera virtualCamera;

    public UnityEvent OnAttackOne;
    public UnityEvent OnAttackTwo;


    public void AttackOne(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("Attack One");
            OnAttackOne.Invoke();
        }
    }

    public void AttackTwo(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnAttackTwo.Invoke();
        }
    }

    public void InstantiateAttackEffect(GameObject vfx)
    {
        GameObject projectile = Instantiate(vfx, attackPoint.position, attackPoint.rotation) as GameObject;
        if(projectile.GetComponent<Bullet>() != null)
        {
            Vector3 shootPosition = virtualCamera.transform.forward;
            //Debug.Log(shootPosition);
            projectile.GetComponent<Bullet>().SetDirection(shootPosition);
        }
    }

    public void InstantiateAttackEffectAtHand(GameObject vfx)
    {
        GameObject projectile = Instantiate(vfx, handPositionAttackPoint.position, Quaternion.identity) as GameObject;
        if(projectile.GetComponent<Bullet>() != null)
        {
            Vector3 shootPosition = virtualCamera.transform.forward;
            //Debug.Log(shootPosition);
            projectile.GetComponent<Bullet>().SetDirection(shootPosition);
        }
    }

    public void InstantiateBullet(GameObject go)
    {
        GameObject projectile = Instantiate(go, attackPoint.position, Quaternion.LookRotation(virtualCamera.transform.forward)) as GameObject;
        if(projectile.GetComponent<Bullet>() != null)
        {
            Vector3 shootPosition = virtualCamera.transform.forward;
            Debug.Log(shootPosition);
            Debug.DrawRay(attackPoint.position, shootPosition.normalized, Color.red, 15f);
            //Debug.Log(shootPosition);
            projectile.GetComponent<Bullet>().SetDirection(shootPosition);
        }
    }
}
