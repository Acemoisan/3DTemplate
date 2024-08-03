using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manager", menuName = "Scriptable Objects/Managers/Time Manager")]
public class TimeManagerSO : ScriptableObject
{
    public float second;
    public float minute;
    public float hour;
    public float dayOfTheMonthIndex;
    public float dayOfTheWeekIndex;
    public string dayOfTheWeekString;
    public float monthIndex;
    public float year;
    public float secondsPerInGameTenMinutes;


    public void DefaultTime()
    {
        TimeManager.Instance.DefaultStartTime();
    }
    public void SkipToNextMorning()
    {
        TimeManager.Instance.SkipToNextMorning();
    }

    public void ManuallySetTime(int hour, int minute = 0)
    {
        TimeManager.Instance.ManuallySetTime(hour, minute);
    }

    public void ManuallySetDay(int monthIndex, int dayOfTheMonthIndex, int yearIndex)
    {
        TimeManager.Instance.ManuallySetDay(monthIndex, dayOfTheMonthIndex, yearIndex);
    }

    public void SetTimeIncrement(int time)
    {
        TimeManager.Instance.SetTimeIncrement(time);
    }

    public void PauseTime(bool pause)
    {
        if(TimeManager.Instance == null) { Debug.Log($"Time Manager is null."); return;}
        TimeManager.Instance.PauseTime(pause);
    }

    public void SetTimeScale(float scale)
    {
        if(TimeManager.Instance == null) { Debug.Log($"Time Manager is null."); return;}
        TimeManager.Instance.SetTimeScale(scale);
    }

    public float GetDaylightPercentage()
    {
        return TimeManager.Instance.GetDaylightPercentage();
    }

    public float GetDaylightSeconds()
    {
        return TimeManager.Instance.GetDaylightSeconds();
    }

    public float GetTotalSeconds()
    {
        return TimeManager.Instance.GetTotalSecondsInADay();
    }
}
