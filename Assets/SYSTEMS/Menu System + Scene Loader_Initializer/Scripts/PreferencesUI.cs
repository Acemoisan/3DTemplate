using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesUI : MonoBehaviour
{
    [Header("Dependencies")]
    public PreferencesSystem preferencesSystem;
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider ambientSlider;


    // [Header("Extras")]
    // public PlayerControllerSO playerControllerSO;
    // public Slider lookSpeedSlider;


    private void Start()
    {
        this.UpdateUI(
            this.preferencesSystem.volumeMusic,
            this.preferencesSystem.volumeSound,
            this.preferencesSystem.volumeAmbient
        );

        //UpdateLookSpeed();
    }

    public void UpdateUI(
        float volumeMusic,
        float volumeSound,
        float volumeAmbient)
    {
        this.musicSlider.value = volumeMusic;
        //this.soundSlider.value = volumeSound;
        //this.ambientSlider.value = volumeAmbient;
    }

    // public void UpdateLookSpeed()
    // {
    //     this.lookSpeedSlider.value = this.playerControllerSO.lookSpeed;
    // }
}
