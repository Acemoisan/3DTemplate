using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aiming;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Aim Settings")]
		public bool allowedToAim;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Events")]
		public UnityEvent OnAttackOne;
		public UnityEvent OnAttackTwo;


		void Start()
		{
			SetCursorState(cursorLocked);
		}
		
		public void OnMovement(InputAction.CallbackContext value)
		{
			MoveInput(value.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.ReadValue<Vector2>());
			}
		}

		public void OnAim(InputAction.CallbackContext value)
		{
			if(allowedToAim == false) return;
			aiming = value.performed;
		}

		public void OnJump(InputAction.CallbackContext value)
		{
			JumpInput(value.performed);
		}

		bool sprinting = false;
		public void OnSprint(InputAction.CallbackContext value)
		{
			sprint = value.performed;
			// if (!value.performed) return;

			// Debug.Log("SprintInput");
			// if(sprinting == false)
			// {
			// 	sprinting = true;
			// 	SprintInput(true);
			// }
			// else
			// {
			// 	sprinting = false;
			// 	SprintInput(false);
			// }
		}


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void AttackOne(InputAction.CallbackContext value)
		{
			if (value.started)
			{
				OnAttackOne.Invoke();
			}
		}

		public void AttackTwo(InputAction.CallbackContext value)
		{
			if (value.started)
			{
				if(allowedToAim) return;

				OnAttackTwo.Invoke();
			}
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		// private void OnApplicationFocus(bool hasFocus)
		// {
		// 	SetCursorState(cursorLocked);
		// }

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}