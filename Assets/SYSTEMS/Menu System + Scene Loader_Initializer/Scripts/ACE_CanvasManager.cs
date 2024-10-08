using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ACE_CanvasManager : MonoBehaviour
{
    [Header("Game Over Event(s)")]
    public  UnityEvent OnGameOverEvent;


    [Header("Toggle Pause Events")]
    public  UnityEvent togglePauseOn;
    public  UnityEvent togglePauseOff;
    bool paused = false;



    public void TogglePause() { 
        if (paused) { togglePauseOff.Invoke(); paused = false; } 
        else { togglePauseOn.Invoke(); paused = true; } 
    }
    public void TogglePauseOn() { togglePauseOn.Invoke(); paused = true; }
    public void TogglePauseOff() { togglePauseOff.Invoke(); paused = false; }
    public void OnGameOver() { OnGameOverEvent.Invoke(); }
}
