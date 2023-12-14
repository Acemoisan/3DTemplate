using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

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
}
