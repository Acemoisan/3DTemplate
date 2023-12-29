using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DaylightManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Light globalLight;


    [Header("Daylight Colors")]
    [SerializeField] Color dayTimeColor;
    [SerializeField] Color nightTimeColor;


    [Header("Seasonal Curves")]
    //[SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] AnimationCurve dayLightCurve;
    //[SerializeField] AnimationCurve spring_SummerCurve;
    


    //AnimationCurve seasonalCurve = new AnimationCurve();



    private void Start()
    {
        InvokeRepeating("UpdateColor", 1, 1);
    } 


    public void UpdateColor()
    {
        // if (SeasonManager.instance.GetSeason() == Season.Winter || SeasonManager.instance.GetSeason() == Season.Fall)
        // {
        //     seasonalCurve = winter_FallCurve;
        // }
        // else if (SeasonManager.instance.GetSeason() == Season.Spring || SeasonManager.instance.GetSeason() == Season.Summer)
        // {
        //     seasonalCurve = spring_SummerCurve;
        // }


        float percentageTime = TimeManager.instance.GetDaylightPercentage();
        float v = dayLightCurve.Evaluate(percentageTime);
        Debug.Log($"Daylight Percentage: {percentageTime} / {v}");
        Color c = Color.Lerp(dayTimeColor, nightTimeColor, v);
        globalLight.color = c;
    }


    public void SetGlobalLight(Light globalLight)
    {
        this.globalLight = globalLight;
    }
}
