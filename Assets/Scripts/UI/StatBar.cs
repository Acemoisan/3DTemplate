using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Slider _statBarSlider;

    void SetMax(float max)
    {
        _statBarSlider.maxValue = max;
        _statBarSlider.minValue = 0f;
    }


    public void UpdateSliderBarValue(float maxValue, float currentValue)
    {
        SetMax(maxValue);
        _statBarSlider.value = currentValue;
    }


    // public void UpdateExpendableEnergyStatBarValue(PlayerAttributes playerAttributes)
    // {
    //     _statBarSlider.value = playerAttributes.GetExpendableEnergy();
    // }    
}
