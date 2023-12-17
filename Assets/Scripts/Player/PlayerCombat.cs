using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Transform attackPoint;
    [SerializeField] Camera virtualCamera;
    [SerializeField] StarterAssetsInputs _input;
    [SerializeField] GameObject debugTransform;
    [SerializeField] LayerMask aimCollider;

    public UnityEvent OnAttackOne;
    public UnityEvent OnAttackTwo;
    Vector3 aimWorldPosition;

    [Header("Debug Settings")]
    [SerializeField] float debugRayRange;
    [SerializeField] LineRenderer baseLineRenderer;
    [SerializeField] LineRenderer aimLineRenderer;


    public void AttackOne()
    {
        OnAttackOne?.Invoke();
    }

    public void AttackTwo()
    {
        OnAttackTwo?.Invoke();
    }


    public void InstantiateBullet(GameObject go)
    {
        Vector3 aimDir; 
        
        if(_input.aiming)
        {
            aimDir = (aimWorldPosition - attackPoint.position).normalized;
            Debug.DrawRay(attackPoint.position, aimDir, Color.blue, debugRayRange);
        }
        else
        {
            aimDir = virtualCamera.transform.forward;
            //Debug.DrawRay(attackPoint.position, shootPosition.normalized, Color.yellow, debugRayRange);
        }

        GameObject projectile = Instantiate(go, attackPoint.position, Quaternion.LookRotation(aimDir, Vector3.up)) as GameObject;
        Debug.Log("Shooting in direction: " + aimDir + ". AIMING: " + _input.aiming +". Check Debug Ray for visual.");
        //projectile.GetComponent<Bullet>().SetDirection(shootPosition);
        

    }

    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = virtualCamera.ScreenPointToRay(screenCenter);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, aimCollider))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            debugTransform.transform.position = hit.point;
            aimWorldPosition = hit.point;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * debugRayRange, Color.red);
        }

        // if(_input.aiming)
        // {
        //     baseLineRenderer.SetActive(false);
        //     aimLineRenderer.SetActive(true);
        //     aimLineRenderer.transform.position = attackPoint.position;
        //     aimLineRenderer.transform.LookAt(aimWorldPosition);
        // }
        // else
        // {
        //     baseLineRenderer.SetActive(true);
        //     aimLineRenderer.SetActive(false);
        //     baseLineRenderer.transform.position = attackPoint.position;
        //     baseLineRenderer.transform.LookAt(attackPoint.position + virtualCamera.transform.forward);
        // }

            aimLineRenderer.transform.position = attackPoint.position;
            aimLineRenderer.transform.LookAt(aimWorldPosition);
            baseLineRenderer.transform.position = attackPoint.position;
            baseLineRenderer.transform.LookAt(attackPoint.position + virtualCamera.transform.forward);
    }
}
