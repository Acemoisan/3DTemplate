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
    [SerializeField] CameraModeController _cameraModeController;

    public UnityEvent OnAttackOne;
    public UnityEvent OnAttackTwo;
    public UnityEvent AimEvent;
    public UnityEvent StopAimEvent;



    //these are in invoked from StarterAssetsInput
    //TODO: Currently ATTACKS are not being used
    // public void AttackOne()
    // {
    //     OnAttackOne?.Invoke();
    // }

    // public void AttackTwo()
    // {
    //     OnAttackTwo?.Invoke();
    // }

    public void Aim()
    {
        AimEvent?.Invoke();
    }

    public void StopAim()
    {
        StopAimEvent?.Invoke();
    }

    public void InstantiateBullet(GameObject go)
    {
        Vector3 aimDir; 
        Vector3 attackPoint = _cameraModeController.AttackPoint.position;
        Vector3 aimPos = _cameraModeController.AimWorldPosition;
        
        if(_input.aiming)
        {
            aimDir = (aimPos - attackPoint).normalized;
        }
        else
        {
            aimDir = _cameraModeController.GetCamera().transform.forward;
        }

        Instantiate(go, attackPoint, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
