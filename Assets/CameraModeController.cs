using System.Collections;
using System.Collections.Generic;
using Acemoisan.Utils;
using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;


public enum CameraModes
{
    GodOfWar,
    LastOfUs,
    AnimalCrossing,
    Ark
}


public class CameraModeController : MonoBehaviour
{
    public Camera _mainCamera;
    public Camera GetCamera() { return _mainCamera; }
    [SerializeField] protected StarterAssetsInputs _input;
    [SerializeField] protected PlayerControllerSO _playerControllerSO;
    [SerializeField] protected PlayerInput _playerInput;



    [Space(20)]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Title("Camera Settings")]
    [EnumToggleButtons] public CameraModes cameraMode;
    public CameraModes GetCameraMode() { return cameraMode; }
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;



    [Space(20)]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [GUIColor(0.8f, 0.3f, 0.8f, 1f)]
    public GameObject CinemachineCameraTarget;   



    [Space(20)]
    [Header("TopDown")]     
    [ShowIf("@this.cameraMode == CameraModes.AnimalCrossing")] public GameObject topDownCamera;


    [Space(20)]
    [Header("Third Person / Ark")]     
    [ShowIf("@this.cameraMode == CameraModes.Ark")] public GameObject rpgCamera;
    [ShowIf("@this.cameraMode == CameraModes.Ark")] public GameObject rpgAimCamera;


    [Space(20)]
    [Header("OTS")]     
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public GameObject overTheShoulderCamera;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public GameObject overTheShoulderAimCamera;  


    [Header("Misc")]  
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs || this.cameraMode == CameraModes.Ark")] public float TopClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs || this.cameraMode == CameraModes.Ark")] public float BottomClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs || this.cameraMode == CameraModes.Ark")] public float CameraAngleOverride = 0.0f;



    [Space(20)]
    [Header("General Mode Settings")]
    [ShowIf("@this.cameraMode != CameraModes.AnimalCrossing")] public GameObject aimCursor;
    [SerializeField] Transform attackPointTransform;
    [SerializeField] LayerMask aimCollider;



    [Space(20)]
    [Title("Debug Settings")]
    [SerializeField] bool debugRays = false;
    //[SerializeField] float debugRayRange;
    [SerializeField] LineRenderer baseLineRenderer;
    [SerializeField] LineRenderer aimLineRenderer;
    [SerializeField] GameObject debugTransform;





    // private variables
    public Transform AttackPoint { get { return attackPointTransform; } }
    Vector3 aimWorldPosition;
    public Vector3 AimWorldPosition { get { return aimWorldPosition; } }
    public float RotationVelocity;
    float _targetRotation = 0.0f;
    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;
    const float _threshold = 0.01f;
    bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
        }
    }




    void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        StartCoroutine(CheckCameraMode());
    }



    // Update is called once per frame
    void Update()
    {
        if(cameraMode == CameraModes.LastOfUs ||
           cameraMode == CameraModes.GodOfWar ||
           cameraMode == CameraModes.Ark)
        {
            if(AceUtils.GetMouseWorldPosition3D(_mainCamera, aimCollider) != Vector3.zero)
            {
                aimWorldPosition = AceUtils.GetMouseWorldPosition3D(_mainCamera, aimCollider);
            }
            

            if(debugRays)
            {
                if(cameraMode == CameraModes.GodOfWar && _input.aiming
                || cameraMode == CameraModes.Ark && _input.aiming
                || cameraMode == CameraModes.LastOfUs)
                {
                    debugTransform.SetActive(true);
                    aimLineRenderer.enabled = true;
                    baseLineRenderer.enabled = true;
                    aimLineRenderer.transform.position = attackPointTransform.position;
                    aimLineRenderer.transform.LookAt(aimWorldPosition);
                    baseLineRenderer.transform.position = attackPointTransform.position;
                    baseLineRenderer.transform.LookAt(attackPointTransform.position + _mainCamera.transform.forward);                
                    debugTransform.transform.position = aimWorldPosition;
                }
                else 
                {
                    debugTransform.SetActive(false);
                    aimLineRenderer.enabled = false;
                    baseLineRenderer.enabled = false;
                }
            }
        }

        if(cameraMode == CameraModes.GodOfWar || cameraMode == CameraModes.LastOfUs)
        {
            AimOverTheShoulderCamera();
        }
    }


    void LateUpdate()
    {
        if (LockCameraPosition) return;
        if (cameraMode == CameraModes.AnimalCrossing) return;
        CameraRotation();
    }   


IEnumerator CheckCameraMode()
{
    while (true)
    {
        overTheShoulderCamera.SetActive(cameraMode == CameraModes.GodOfWar || cameraMode == CameraModes.LastOfUs);
        overTheShoulderAimCamera.SetActive(cameraMode == CameraModes.GodOfWar || cameraMode == CameraModes.LastOfUs);
        topDownCamera.SetActive(cameraMode == CameraModes.AnimalCrossing);
        rpgCamera.SetActive(cameraMode == CameraModes.Ark);
        rpgAimCamera.SetActive(cameraMode == CameraModes.Ark);
        aimCursor.SetActive(cameraMode == CameraModes.Ark || cameraMode == CameraModes.GodOfWar || cameraMode == CameraModes.LastOfUs);

        yield return new WaitForSeconds(.5f);
    }
}


    public void SetCameraMode(CameraModes mode)
    {
        cameraMode = mode;
    }


public void AimOverTheShoulderCamera()
{
    if (LockCameraPosition) return;

    bool isGodOfWarOrLastOfUs = cameraMode == CameraModes.GodOfWar || cameraMode == CameraModes.LastOfUs;
    bool isArk = cameraMode == CameraModes.Ark;

    overTheShoulderCamera.SetActive(isGodOfWarOrLastOfUs && !_input.aiming);
    overTheShoulderAimCamera.SetActive(isGodOfWarOrLastOfUs && _input.aiming);
    rpgCamera.SetActive(isArk && !_input.aiming);
    rpgAimCamera.SetActive(isArk && _input.aiming);
    aimCursor.SetActive((isGodOfWarOrLastOfUs || isArk) && _input.aiming);
}


    void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime * _playerControllerSO.lookSpeed;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }


        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }   


    float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }    
}
