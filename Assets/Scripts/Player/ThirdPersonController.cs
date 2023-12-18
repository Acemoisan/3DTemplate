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





        [Header("Debug Settings")]
        [SerializeField] float debugRayRange;
        [SerializeField] LineRenderer baseLineRenderer;
        [SerializeField] LineRenderer aimLineRenderer;
        [SerializeField] GameObject debugTransform;
        [SerializeField] LayerMask aimCollider;
        Vector3 aimWorldPosition;
        public Vector3 AimWorldPosition { get { return aimWorldPosition; } }
        [SerializeField] Transform attackPoint;
        public Transform AttackPoint { get { return attackPoint; } }




        // private variables
        private float _animationBlend;
        private float _targetRotation = 0.0f;


        // animation IDs

        private bool _hasAnimator;





        public override void Start()
        {
            TopClamp = 70.0f;
            BottomClamp = -30.0f;

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = _animator != null;

            base.Start();
        }

        public override void Update()
        {
            _hasAnimator = _animator != null;
            base.Update();

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
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


            aimLineRenderer.transform.position = attackPoint.position;
            aimLineRenderer.transform.LookAt(aimWorldPosition);
            baseLineRenderer.transform.position = attackPoint.position;
            baseLineRenderer.transform.LookAt(attackPoint.position + _mainCamera.transform.forward);
        }


        protected override void CameraRotation()
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


        protected override void HandleMovement()
        {
            if (cameraMode == CameraModes.OverTheShoulder || cameraMode == CameraModes.OverTheShoulderLockRotation)
            {
                inputDirection = inputDirection.x * _mainCamera.transform.right + inputDirection.z * _mainCamera.transform.forward;
                inputDirection.y = 0.0f;
                _controller.Move(inputDirection * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            } 
            else if (cameraMode == CameraModes.TopDown)
            {
                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
                _controller.Move(targetDirection * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            
        }    

        protected override void HandlePlayerObjectRotation()
        {
            //ROTATE BASED ON CAMERA (180 TURNS (OVER THE SHOULDER))
            if (cameraMode == CameraModes.OverTheShoulder || cameraMode == CameraModes.OverTheShoulderLockRotation)
            {
                _targetRotation = _mainCamera.transform.eulerAngles.y;
            }

            //ROTATE BASED ON INPUT MOVEMENT (360 TURN ON THE SPOT) //if CAMERA MODE IS TOPDOWN
            if (cameraMode == CameraModes.TopDown)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            }
            

            //ROTATE BASED ON INPUT MOVEMENT
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            //ROTATE PLAYER WHEN MOVING. OR WHEN ROTATEWHILESTILL BOOL IS TRUE
            if (_input.move != Vector2.zero || _rotateBodyWhenStandingStill) 
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
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