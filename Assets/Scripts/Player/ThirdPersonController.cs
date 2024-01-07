using Sirenix.OdinInspector;
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
        [Title("THIRD PERSON --------------")]
        [SerializeField] Animator _animator;


        // private variables
        private bool _hasAnimator;





        public override void Start()
        {
            _hasAnimator = _animator != null;
            base.Start();
        }

        public override void Update()
        {
            _hasAnimator = _animator != null;
            base.Update();
        }


        protected override void HandleMovement()
        {
            if (_cameraModeController.GetCameraMode() == CameraModes.GodOfWar || 
            _cameraModeController.GetCameraMode() == CameraModes.LastOfUs || 
            _cameraModeController.GetCameraMode() == CameraModes.Ark)
            {
                // Get the camera's forward and right vectors, but ignore the pitch.
                Vector3 cameraForward = _cameraModeController.GetCamera().transform.forward;
                cameraForward.y = 0; // Ignore camera's vertical angle
                Vector3 cameraRight = _cameraModeController.GetCamera().transform.right;

                HandleFlying();


                // Calculate input direction based on camera's horizontal direction
                inputDirection = inputDirection.x * cameraRight + inputDirection.z * cameraForward;
                // if(flying)
                // {
                //     inputDirection.y = inputDirection.y * cameraForward.y;
                // }
                inputDirection.Normalize(); // Optional, to ensure consistent movement speed

                _controller.Move(inputDirection * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            } 
            else if (_cameraModeController.GetCameraMode() == CameraModes.AnimalCrossing)
            {
                float _targetRot = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cameraModeController.GetCamera().transform.eulerAngles.y;

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRot, 0.0f) * Vector3.forward;
                _controller.Move(targetDirection * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            
        }    

        protected override void HandlePlayerObjectRotation()
        {
            //ROTATE BASED ON CAMERA 
            float _targetRotation = _cameraModeController.GetCamera().transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _cameraModeController.RotationVelocity, _cameraModeController.RotationSmoothTime);


            //ROTATE PLAYER WHEN MOVING. OR WHEN IN CERTAIN MODES
            if (_input.move != Vector2.zero || _cameraModeController.GetCameraMode() == CameraModes.LastOfUs || _cameraModeController.GetCameraMode() == CameraModes.Ark) 
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        void HandleFlying()
        {
            if(GameManager.Instance == null) return;

            if(GameManager.Instance.CurrentGameMode == GameMode.Creative && _flying) 
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    _verticalVelocity = _flySpeed;

                    if(!wasFlyingLastFrame) 
                    {
                        SetGravity(0); // Only called once when state changes to flying
                        wasFlyingLastFrame = true;
                    }
                }
                else if(Input.GetKey(KeyCode.LeftControl))
                {
                    _verticalVelocity = -_flySpeed;
                }
                else
                {
                    _verticalVelocity = 0f;
                }

                if(!Grounded)
                {
                  FlyEvent();
                }
            }
            else 
            {
                if(wasFlyingLastFrame)
                {
                    RestoreGravity(); // Only called once when state changes to not flying
                    wasFlyingLastFrame = false;
                }
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
                _animator.SetBool(_animIDFly, false);
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

        protected override void FlyEvent()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDFly, true);
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