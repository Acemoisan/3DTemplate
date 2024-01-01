using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScheme : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    public bool UsingKeyboard()
    {
        return _playerInput.currentControlScheme == "KeyboardMouse";
    }
}
