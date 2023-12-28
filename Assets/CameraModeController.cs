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
    [Space(20)]
    [Header("OTS")]     
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public GameObject overTheShoulderCamera;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public GameObject overTheShoulderAimCamera;    
    //[ShowIf("@this.cameraMode == CameraModes.OverTheShoulderLockRotation")] public GameObject overTheShoulderFarCamera;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public GameObject aimCursor;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs || this.cameraMode == CameraModes.Ark")] public float TopClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs || this.cameraMode == CameraModes.Ark")] public float BottomClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public float CameraAngleOverride = 0.0f;



    [Space(20)]
    [Header("General Mode Settings")]
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
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
    protected float _targetRotation = 0.0f;
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;
    protected const float _threshold = 0.01f;
    protected bool IsCurrentDeviceMouse
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


    //GETTERS
    public float GetTargetRotation() { return _targetRotation; }


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
        //TODO: MOSTLY MEANT FOR TESTING.. WILL BE MOVED TO START() WHEN FINISHED
        while (true)
        {
            //Setting Camera Mode Mods
            switch (cameraMode)
            {
                //GOD Of War: Over the shoulder, 360 Camera rotation around player when still
                case CameraModes.GodOfWar:
                    overTheShoulderCamera.SetActive(true);
                    overTheShoulderAimCamera.SetActive(true);
                    topDownCamera.SetActive(false);
                    rpgCamera.SetActive(false);
                    break;
                //Last Of Us: Over the shoulder, Locked camera rotation to player Forward
                case CameraModes.LastOfUs:
                    overTheShoulderCamera.SetActive(true);
                    overTheShoulderAimCamera.SetActive(true);
                    topDownCamera.SetActive(false);
                    rpgCamera.SetActive(false);
                    break;
                case CameraModes.AnimalCrossing:
                    topDownCamera.SetActive(true);
                    overTheShoulderCamera.SetActive(false);
                    overTheShoulderAimCamera.SetActive(false);
                    rpgCamera.SetActive(false);
                    aimCursor.SetActive(false);
                    break;
                case CameraModes.Ark:
                    rpgCamera.SetActive(true);
                    topDownCamera.SetActive(false);
                    overTheShoulderCamera.SetActive(false);
                    overTheShoulderAimCamera.SetActive(false);
                    aimCursor.SetActive(true);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    public void SetCameraMode(CameraModes mode)
    {
        cameraMode = mode;
    }


    public void AimOverTheShoulderCamera()
    {
        if(LockCameraPosition) return;
        
        if (_input.aiming)
        {
            overTheShoulderCamera.SetActive(false);
            overTheShoulderAimCamera.SetActive(true);
            aimCursor.SetActive(true);
        }
        else
        {
            overTheShoulderCamera.SetActive(true);
            overTheShoulderAimCamera.SetActive(false);
            aimCursor.SetActive(false);
        }    
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
