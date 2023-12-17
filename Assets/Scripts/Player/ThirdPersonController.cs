 using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    public class ThirdPersonController : PlayerController
    {
        [Space(20)]
        [Header("THIRD PERSON --------------")]
        [SerializeField] Animator _animator;


        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        public GameObject mainCamera;
        public GameObject aimCamera;
        public GameObject aimCursor;


        // private variables
        private float _animationBlend;
        private float _targetRotation = 0.0f;


        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private bool _hasAnimator;



        public override void Awake()
        {
            base.Awake();
        }


        public override void Start()
        {
            TopClamp = 70.0f;
            BottomClamp = -30.0f;

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = _animator != null;

            AssignAnimationIDs();

            base.Start();
        }

        public override void Update()
        {
            _hasAnimator = _animator != null;
            base.Update();
            AimCamera();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        public void AimCamera()
        {
            if (_input.aiming)
            {
                mainCamera.SetActive(false);
                aimCamera.SetActive(true);
                aimCursor.SetActive(true);
            }
            else
            {
                mainCamera.SetActive(true);
                aimCamera.SetActive(false);
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


        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
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


        protected override void HandleMovement()
        {
            inputDirection = inputDirection.x * _mainCamera.transform.right + inputDirection.z * _mainCamera.transform.forward;
            inputDirection.y = 0.0f;
            _controller.Move(inputDirection * (_speed * Time.deltaTime) +
            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }    

        protected override void HandlePlayerObjectRotation()
        {
            _targetRotation = _mainCamera.transform.eulerAngles.y;

            //ROTATE BASED ON INPUT MOVEMENT
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            //rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        protected override void HandleAnimator()
        {
            // update animator if using character
            if (_hasAnimator)
            {
                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _playerControllerSO.SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }


        protected override void GroundedEvent()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }
        }  

        protected override void FreeFallEvent()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDFreeFall, true);
            }        
        }

        protected override void JumpEvent()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, true);
            }        
        }

        protected override void GroundCheckEvent()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}