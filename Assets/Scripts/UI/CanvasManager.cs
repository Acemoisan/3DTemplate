using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] HUDUI hudReference;

    [Header("Canvas Events")]
    public UnityEvent OnPlay;
    public UnityEvent OnDeath;
    public UnityEvent OnPause;
    public UnityEvent OnInventory;
    public UnityEvent OnDialogue;
    public UnityEvent OnDebug;
    public UnityEvent OnEndGame;



    public void EnablePlayerInput(bool enable)
    {
        playerInput.enabled = enable;
    }


    public void Play()
    {
        //Debug.Log("Play");
        OnPlay?.Invoke();
    }

    public void Pause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnPause?.Invoke();
        }
    }

    public void Inventory(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnInventory?.Invoke();
        }
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void Dialogue()
    {
        OnDialogue?.Invoke();
    }

    public void Debug(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnDebug?.Invoke();
        }
    }

    public void NextHotbarButton(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        hudReference.NextButton();
    }

    public void PreviousHotbarButton(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        hudReference.PreviousButton();
    }

    float numberKey;
    public void SelectHotBarSlotWithNumberKeys(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (context.ReadValue<float>() == 0)
        {
            numberKey = 9;
        }else
        {
            numberKey = context.ReadValue<float>() - 1;
        }

        hudReference.SelectHotbarSlotWithKeyboard(numberKey);
    }

    public void SelectHotbarWithMouse(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        float delta = context.ReadValue<float>();
        hudReference.SelectButtonWithScrollWheel(delta);   
    }    
}
