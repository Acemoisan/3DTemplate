using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : MonoBehaviour
{
        [Header("Dependencies")]
		[SerializeField] protected PlayerInput _playerInput;
		[SerializeField] protected CharacterController _controller;
		[SerializeField] protected StarterAssetsInputs _input;
        [SerializeField] protected PlayerControllerSO _playerControllerSO;


		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;



		[Header("Player Audio")]
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;



        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;        
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp;


        #region Private Variables
        //Private Variables
        protected float _speed;
		protected float _rotationVelocity;
		protected float _verticalVelocity;
        protected float _terminalVelocity = 53.0f;
        protected float _jumpTimeoutDelta;
        protected float _fallTimeoutDelta;
        protected const float _threshold = 0.01f;
        protected Vector3 inputDirection;
        protected float inputMagnitude;
        protected float targetSpeed;
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
        protected GameObject _mainCamera;
        protected float _cinemachineTargetYaw;
        protected float _cinemachineTargetPitch;
        #endregion



        #region Methods
        public virtual void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (_mainCamera == null)
                {
                    Debug.LogWarning("MainCamera tag not found. Remember to tag the main camera with 'MainCamera'");
                }
            }
        }


        public virtual void Start()
        {
            _jumpTimeoutDelta = _playerControllerSO.JumpTimeout;
            _fallTimeoutDelta = _playerControllerSO.FallTimeout;
        }


        public virtual void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }


        protected void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            targetSpeed = _input.sprint ? _playerControllerSO.SprintSpeed : _playerControllerSO.MoveSpeed;

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


            if (_input.move != Vector2.zero)
            {
                HandlePlayerObjectRotation(); // This method will be overridden by subclasses to handle rotation differently
            }
            
            //HandlePlayerObjectRotation(); // This method will be overridden by subclasses to handle rotation differently

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
                    _verticalVelocity = Mathf.Sqrt(_playerControllerSO.JumpHeight * -2f * _playerControllerSO.Gravity);

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
}
