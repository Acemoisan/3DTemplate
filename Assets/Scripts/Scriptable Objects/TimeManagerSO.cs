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

    public void SkipToNextMorning()
    {
        TimeManager.instance.SkipToNextMorning();
    }

    public void SetTime(int time)
    {
        TimeManager.instance.SetTime(time);
    }

    public void SetTimeIncrement(int time)
    {
        TimeManager.instance.SetTimeIncrement(time);
    }

    public void PauseTime(bool pause)
    {
        TimeManager.instance.PauseTime(pause);
    }
}
