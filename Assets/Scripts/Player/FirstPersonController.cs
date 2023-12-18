using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{

	public class FirstPersonController : PlayerController
	{

		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;




		public override void Start()
		{
			TopClamp = 90f;
			BottomClamp = -90f;
			base.Start();
		}

		public override void Update()
		{
			base.Update();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}


		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime * _playerControllerSO.lookSpeed;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}


	    protected override void HandleMovement()
        {
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        }    

        protected override void HandlePlayerObjectRotation()
        {
			inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
        }

        protected override void HandleAnimator(){}

	}
}