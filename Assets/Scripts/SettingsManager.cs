using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Toggle fullScreenToggle;
    bool isFullScreen;
    Resolution[] resolutions;


    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height) 
            { 
                currentResolutionIndex = i; 
            }
            Debug.Log("Resolution: " + option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }



    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
    }

    public void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ToggleFullScreen()
    {
        isFullScreen = fullScreenToggle.isOn;
        Screen.fullScreen = isFullScreen;
    }
}
