/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

/// <summary>
/// There are 22 hours per day (6am -> 3am + 13th hour)
/// </summary>
public class TimeManager : MonoBehaviour
{
    [SerializeField] TimeManagerSO timeManagerSO;

    [Header("Day Values")]
    public int daysPlaying = 1;
    public int dayOfTheMonthIndex;
    public int dayOfTheWeekIndex = 0;
    public int monthIndex;
    //public int seasonIndex;
    public int year;
    public string dayOfTheWeekString;
    [SerializeField] List<string> daysOfTheWeek;


    [Header("Time Values")]
    public int hour;
    public int minute;
    public int second;
    public int secondsPerInGameTenMinutes;
    public bool timePaused;


    [Header("Time Phases")]
    public string seasonString;
    public float morning;
    public float midDay;
    public float evening;
    public float twilight;
    public float night;


    [Header("Realtime Played")]
    public int realMinutes;
    public int realHours;
    public string timeSaved;
    public string dateSaved;


    public int daylightSeconds;


    [Header("Events")]
    [SerializeField] UnityEvent dayCompleteEvent;
    [SerializeField] UnityEvent changeSeasonEvent;
    [SerializeField] UnityEvent monthCompleteEvent;
    [SerializeField] UnityEvent yearCompleteEvent;
    [SerializeField] UnityEvent hourUpdatedEvent;
    [SerializeField] UnityEvent minuteUpdatedEvent;
    [SerializeField] UnityEvent tenMinutesPassed;
    [SerializeField] UnityEvent newPhaseOfDay;

    public GameEvent onHourUpdated;


    public static TimeManager instance;



    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        StartCoroutine(IncreaseTime());
        //InvokeRepeating("IncreaseRealTime", 60, 60);
        //Invoke("DelaySetDaylightTime", 1);
    }  


    void DelaySetDaylightTime()
    {
        //SetDaylightTime();
    }


    void IncreaseRealTime()
    {
        realMinutes++;
        if(realMinutes >= 60)
        {
            realMinutes = 0;
            realHours++;
        }
    }


    IEnumerator IncreaseTime()
    {

        while(true)
        {
            if(TimePaused() == false)
            {
                second++;

                if (second >= secondsPerInGameTenMinutes)
                {
                    second = 0;

                    RaiseTenMinutesPassedEvent();


                    if (minute < 50)
                    {
                        minute += 10;
                    }
                    else
                    {
                        minute = 0;
                        if (hour < 23)
                        {
                            hour++;
                        }else
                        {
                            hour = 0;

                            NextDay();
                        }


                        // if (hour == 3)
                        // {
                        //     NextDay();
                        // }


                        RaiseHourUpdateEvent();
                        SetDaylightTime();
                        FindPhaseOfDay();
                    }

                    RaiseMinuteUpdateEvent();
                }
            }
            UpdateTimeOnTimeManagerSO();

            yield return new WaitForSeconds(1f);
        }
    }


    void FindPhaseOfDay()
    {
        if (hour == morning || hour == midDay || 
        hour == evening || hour == twilight || hour == night)
        {
            RaiseNewPhaseOfDayEvent();
        }
    }


    public void NextDay()
    {
        daysPlaying++;

        IncreaseDayOfTheMonth();

        IncreaseDayOfTheWeek();

        RaiseDayCompleteEvent();
        
        UpdateTimeOnUI();
    }

    public void SkipToNextMorning()
    {
        hour = 6;
        minute = 0;
        second = 0;
        NextDay();
        SetDaylightTime();
    }

    public void SetTime(int time)
    {
        hour = time;
        minute = 0;
        second = 0;
        SetDaylightTime();
        UpdateTimeOnUI();
        RaiseHourUpdateEvent();
    }

    public void SetTimeIncrement(int inc)
    {
        secondsPerInGameTenMinutes = inc;
    }

    public void PauseTime(bool pause)
    {
        timePaused = pause;

        if (timePaused)
        {
            SetTimeScale(0);
        }
        else
        {
            SetTimeScale(1);
        }
    }
    
    public void DefaultTimeIncrement()
    {
        secondsPerInGameTenMinutes = 8;
    }

    private void IncreaseDayOfTheWeek()
    {
        Debug.Log("Day of the week increased");
        dayOfTheWeekIndex++;

        if (dayOfTheWeekIndex > 6)
        {
            dayOfTheWeekIndex = 0;
        }
        dayOfTheWeekString = daysOfTheWeek[dayOfTheWeekIndex];
    }

    private void IncreaseDayOfTheMonth()
    {
        dayOfTheMonthIndex++;

        if (dayOfTheMonthIndex > 30)
        {
            dayOfTheMonthIndex = 1;
            NextMonth();
        }
    }

    void NextMonth()
    {
        //MAX 4 MONTHS // 1 MONTH PER SEASON
        monthIndex++;
        RaiseMonthCompleteEvent();

        if (monthIndex > 4)
        {
            monthIndex = 0;
            RaiseYearCompleteEvent();
        }

        // seasonIndex++;
        // if (seasonIndex == 4)
        // {
        //     seasonIndex = 0;
        //     SendSeasonCompleteEvent();
        //     year++;
        //     RaiseYearCompleteEvent();
        // }
        RaiseSeasonCompleteEvent();
    }

    public void RaiseMinuteUpdateEvent()
    {
        //minuteUpdatedEvent.Raise();
    }

    public void RaiseTenMinutesPassedEvent()
    {
        //tenMinutesPassed.Raise();
    }

    public void RaiseHourUpdateEvent()
    {
        //hourUpdatedEvent.Raise();
        onHourUpdated.Raise();
    }

    public void RaiseDayCompleteEvent()
    {
        //dayCompleteEvent.Raise();
    }

    public void RaiseNewPhaseOfDayEvent()
    {
        //newPhaseOfDay.Raise();
    }

    void RaiseSeasonCompleteEvent()
    {
        //changeSeasonEvent.Raise();
    }

    void RaiseMonthCompleteEvent()
    {
        //monthCompleteEvent.Raise();
    }

    void RaiseYearCompleteEvent()
    {
        //yearCompleteEvent.Raise();
    }

    public void UpdateTimeOnUI()
    {
        RaiseHourUpdateEvent();
        RaiseMinuteUpdateEvent();
    }

    public void UpdateTimeOnTimeManagerSO()
    {
        timeManagerSO.hour = hour;
        timeManagerSO.minute = minute;
        timeManagerSO.second = second;
        timeManagerSO.dayOfTheMonthIndex = dayOfTheMonthIndex;
        timeManagerSO.dayOfTheWeekIndex = dayOfTheWeekIndex;
        timeManagerSO.dayOfTheWeekString = dayOfTheWeekString;
        timeManagerSO.monthIndex = monthIndex;
        timeManagerSO.year = year;
    }
    
    public void SaveRealTime()
    {
        timeSaved = System.DateTime.Now.ToString("HH:mm:ss");
        dateSaved = System.DateTime.Now.ToString("yyyy/MM/dd");
    }

    public void SetDaylightTime()
    {
        daylightSeconds = (hour * 8) * 6;
    }
    
    public int GetDaylightSeconds()
    {
        //THERE ARE 1152 SECONDS IN AN IN GAME DAY -- AT 8 SECONDS PER HOUR. (8 * 6) * 24 = 1152
        int hourInSeconds = (secondsPerInGameTenMinutes * 6) * hour;
        int additionalMinutes = (minute / 10) * secondsPerInGameTenMinutes;
        return hourInSeconds + additionalMinutes + second;
    }

    public float GetDaylightPercentage()
    {
        float totalSecondsInDay = (secondsPerInGameTenMinutes * 6) * 24;
        return (float)GetDaylightSeconds() / totalSecondsInDay;
    }

    public void OnTimeScaleRequested(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    public bool TimePaused()
    {
        return timePaused;
    }

    public int GetMonthIndex()
    {
        return monthIndex;
    }

    public int GetDayOfTheMonthIndex()
    {
        return dayOfTheMonthIndex;
    }

    public float GetSecondsPerTenMin()
    {
        return secondsPerInGameTenMinutes;
    }

    public float GetYear()
    {
        return year;
    }

    public float GetSecond()
    {
        return second;
    }

    public float GetMinute()
    {
        return minute;
    }

    public float GetHour()
    {
        return hour;
    }
}