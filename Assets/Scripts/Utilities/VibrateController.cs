using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrateController : MonoBehaviour
{
    [SerializeField] float lowintensity = 0.1f;
    [SerializeField] float highintensity = 1f;

    public void Vibrate()
    {
        StartCoroutine(ControllerVibrate(0.2f));
    }

    IEnumerator ControllerVibrate(float vibrateTime)
    {
        if(Gamepad.current == null) { yield break; }
        Gamepad.current.SetMotorSpeeds(lowintensity, highintensity);
        yield return new WaitForSeconds(vibrateTime);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
