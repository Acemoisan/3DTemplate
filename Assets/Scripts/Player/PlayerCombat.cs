using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Acemoisan.Utils;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] StarterAssetsInputs _input;

    public UnityEvent OnAttackOne;
    public UnityEvent OnAttackTwo;



    //these are in invoked from StarterAssetsInput
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
        Vector3 attackPoint = playerController.AttackPoint.position;
        Vector3 aimPos = playerController.AimWorldPosition;
        
        if(_input.aiming)
        {
            aimDir = (aimPos - attackPoint).normalized;
        }
        else
        {
            aimDir = playerController._mainCamera.transform.forward;
        }

        Instantiate(go, attackPoint, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
