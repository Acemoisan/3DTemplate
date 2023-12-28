using System.Collections;
using System.Collections.Generic;
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


public abstract class PlayerController : MonoBehaviour
{
    [Title("Dependencies")]
    [SerializeField] protected PlayerControllerSO _playerControllerSO;
    [SerializeField] protected PlayerInput _playerInput;
    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected StarterAssetsInputs _input;
    public Camera _mainCamera;



    [Space(20)]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Title("Camera Settings")]
    [EnumToggleButtons] public CameraModes cameraMode;



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
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public float TopClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public float BottomClamp;
    [ShowIf("@this.cameraMode == CameraModes.GodOfWar || this.cameraMode == CameraModes.LastOfUs")] public float CameraAngleOverride = 0.0f;



    [Space(20)]
    [Header("General Mode Settings")]
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask aimCollider;



    [Space(20)]
    [Title("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] protected bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;



    [Space(20)]
    [Title("Player Audio")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;



    [Space(20)]
    [Title("Debug Settings")]
    [SerializeField] bool debugRays = false;
    //[SerializeField] float debugRayRange;
    [SerializeField] LineRenderer baseLineRenderer;
    [SerializeField] LineRenderer aimLineRenderer;
    [SerializeField] GameObject debugTransform;






    #region Private Variables // Getters
    public Transform AttackPoint { get { return attackPoint; } }
    Vector3 aimWorldPosition;
    public Vector3 AimWorldPosition { get { return aimWorldPosition; } }
    //Private Variables
    protected float _speed;
    protected float _rotationVelocity;
    protected float jumpHeight;
    protected float originalJumpHeight;
    protected float gravity;
    protected float originalGravity;
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;
    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;
    protected const float _threshold = 0.01f;
    protected Vector3 inputDirection;
    protected float inputMagnitude;
    protected float targetSpeed;
    protected float walkSpeed;
    protected float sprintSpeed;
    protected float originalWalkSpeed;
    protected float originalSprintSpeed;
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
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;

    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFreeFall;
    protected int _animIDMotionSpeed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    #endregion



    #region Methods
    public virtual void Start()
    {
        _jumpTimeoutDelta = _playerControllerSO.JumpTimeout;
        _fallTimeoutDelta = _playerControllerSO.FallTimeout;
        walkSpeed = _playerControllerSO.MoveSpeed;
        sprintSpeed = _playerControllerSO.SprintSpeedMultiplier * walkSpeed;
        originalWalkSpeed = walkSpeed;
        originalSprintSpeed = sprintSpeed;
        jumpHeight = _playerControllerSO.JumpHeight;
        originalJumpHeight = jumpHeight;
        gravity = _playerControllerSO.Gravity;
        originalGravity = gravity;
        AssignAnimationIDs();

        StartCoroutine(CheckCameraMode());
    }


    public virtual void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();

        if(cameraMode == CameraModes.LastOfUs ||
           cameraMode == CameraModes.GodOfWar ||
           cameraMode == CameraModes.Ark)
        {
            
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000f, aimCollider))
            {
                aimWorldPosition = hit.point;
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
                    aimLineRenderer.transform.position = attackPoint.position;
                    aimLineRenderer.transform.LookAt(aimWorldPosition);
                    baseLineRenderer.transform.position = attackPoint.position;
                    baseLineRenderer.transform.LookAt(attackPoint.position + _mainCamera.transform.forward);                
                    debugTransform.transform.position = hit.point;
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

        public static Vector3 GetMouseWorldPosition3D(Camera camera, int layerMask)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = camera.ScreenPointToRay(screenCenter);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
            {
                return hit.point;
            }
            else 
            {
                return Vector3.zero;
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
                    aimCursor.SetActive(false);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(.5f);
        }
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

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    protected void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        targetSpeed = _input.sprint ? sprintSpeed : walkSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * _playerControllerSO.SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // normalise input direction
        inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;


        
        HandlePlayerObjectRotation();
        
        HandleMovement();

        HandleAnimator();                 

    }   


    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = _playerControllerSO.FallTimeout;


            GroundedEvent();

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                JumpEvent();
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = _playerControllerSO.JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                FreeFallEvent();
            }

            // if we are not grounded, do not jump
            _input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _playerControllerSO.Gravity * Time.deltaTime;
        }
    }


    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        GroundCheckEvent();
    }


    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }




    protected virtual void CameraRotation(){}
    protected abstract void HandleMovement();
    protected virtual void HandlePlayerObjectRotation()
    {
        // By default, do nothing. Subclasses will provide their own implementations if necessary.
    }
    protected virtual void HandleAnimator()
    {
        // Optional: Subclasses override if they need to update an animator
    }
    protected virtual void GroundedEvent() {}
    protected virtual void JumpEvent() {}
    protected virtual void FreeFallEvent() {}
    protected virtual void GroundCheckEvent(){}
    #endregion

    public void SetMoveSpeed(float speed)
    {
        walkSpeed = speed;
        sprintSpeed = walkSpeed * _playerControllerSO.SprintSpeedMultiplier;
    }

    public void SetJumpHeight(float height)
    {
        jumpHeight = height;
    }

    public void SetGravity(float value)
    {
        gravity = value;
    }

    public void RevertStats()
    {
        walkSpeed = originalWalkSpeed;
        sprintSpeed = originalSprintSpeed;
        jumpHeight = originalJumpHeight;
        gravity = originalGravity;
    }
}
