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

    public void SkipToNextMorning()
    {
        TimeManager.instance.SkipToNextMorning();
    }

    public void ManuallySetTime(int hour, int minute = 0)
    {
        TimeManager.instance.ManuallySetTime(hour, minute);
    }

    public void SetTimeIncrement(int time)
    {
        TimeManager.instance.SetTimeIncrement(time);
    }

    public void PauseTime(bool pause)
    {
        TimeManager.instance.PauseTime(pause);
    }

    public float GetDaylightPercentage()
    {
        return TimeManager.instance.GetDaylightPercentage();
    }

    public float GetDaylightSeconds()
    {
        return TimeManager.instance.GetDaylightSeconds();
    }

    public float GetTotalSeconds()
    {
        return TimeManager.instance.GetTotalSecondsInADay();
    }
}
