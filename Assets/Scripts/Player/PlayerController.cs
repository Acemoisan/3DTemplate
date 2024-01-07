using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Acemoisan.Utils;




public abstract class PlayerController : MonoBehaviour
{
    [Title("Dependencies")]
    [SerializeField] protected PlayerControllerSO _playerControllerSO;
    public PlayerControllerSO PlayerControllerSO { get { if(_playerControllerSO == null){ Debug.Log($"No ControllerSO found on {this}"); } return _playerControllerSO; } }
    [SerializeField] protected PlayerInput _playerInput;
    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected StarterAssetsInputs _input;
    [SerializeField] protected CameraModeController _cameraModeController;






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
    [Title("Player Fly")]
    [SerializeField] protected bool _flying = false;
    [SerializeField] protected float _flySpeed = 10f;
    public float FlySpeed { get { return _flySpeed; } }



    [Space(20)]
    [Title("Player Audio")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;










    #region Private Variables // Getters

    //Private Variables
    protected float _speed;
    protected float jumpHeight;
    public float JumpHeight { get { return jumpHeight; } }
    protected float gravity;
    public float Gravity { get { return gravity; } }
    protected bool wasFlyingLastFrame = false;
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;
    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;
    protected Vector3 inputDirection;
    protected float inputMagnitude;
    protected float targetSpeed;
    protected float walkSpeed;
    public float WalkSpeed { get { return walkSpeed; } }
    protected float sprintSpeed;
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

    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFly;
    protected int _animIDFreeFall;
    protected int _animIDMotionSpeed;
    protected float _animationBlend;
    #endregion



    #region Methods
    public virtual void Start()
    {
        RevertStats();
        AssignAnimationIDs();
    }


    public virtual void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }
        
    public void RevertStats()
    {
        _jumpTimeoutDelta = _playerControllerSO.JumpTimeout;
        _fallTimeoutDelta = _playerControllerSO.FallTimeout;
        walkSpeed = _playerControllerSO.OriginalMoveSpeed;
        sprintSpeed = walkSpeed * _playerControllerSO.SprintSpeedMultiplier;
        jumpHeight = _playerControllerSO.OriginalJumpHeight;
        gravity = _playerControllerSO.OriginalGravity;
        _flySpeed = _playerControllerSO.OriginalFlySpeed;
    }    

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFly = Animator.StringToHash("Fly");
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


        // if(inputDirection != Vector3.zero)
        // {
        //     AudioManager.instance.PlaySound(Sound.PlayerAttack);
        // }
        
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
            _verticalVelocity += _playerControllerSO.OriginalGravity * Time.deltaTime;
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
    protected virtual void FlyEvent() {}
    protected virtual void FreeFallEvent() {}
    protected virtual void GroundCheckEvent(){}
    #endregion

    public void SetMoveSpeed(float speed)
    {
        walkSpeed = speed;
        sprintSpeed = walkSpeed * _playerControllerSO.SprintSpeedMultiplier;
    }

    public void SetFlySpeed(float speed)
    {
        _flySpeed = speed;
    }

    public void SetJumpHeight(float height)
    {
        jumpHeight = height;
    }

    public void SetGravity(float value)
    {
        gravity = value;
    }

    public void RestoreGravity()
    {
        gravity = _playerControllerSO.OriginalGravity;
    }

    public void SetFlying(bool flying, float flySpeed)
    {
        _flying = flying;
        _flySpeed = flySpeed;

        if(flying == false)
        {
            RestoreGravity();
        }
    }
}
