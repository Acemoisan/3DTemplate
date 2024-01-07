using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeUI : MonoBehaviour
{
    [SerializeField] TimeManagerSO timeManagerSO;
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] TextMeshProUGUI minuteText;
    [SerializeField] TextMeshProUGUI dayOfTheWeekStringText;
    [SerializeField] TextMeshProUGUI dayIndexText;

    void Start()
    {
        StartCoroutine(UpdateUI());
    }


    public IEnumerator UpdateUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(TimeManager.Instance == null) continue;
            hourText.text = ConvertTo12HourFormat(TimeManager.Instance.hour).ToString("00");
            minuteText.text = ":" + TimeManager.Instance.minute.ToString("00");
            dayOfTheWeekStringText.text = TimeManager.Instance.dayOfTheWeekString;
            dayIndexText.text = TimeManager.Instance.dayOfTheMonthIndex.ToString();
            
        }
    }

    public static float ConvertTo12HourFormat(float hour24)
    {
        // If the hour is 0 or 12, the 12-hour format should show 12.
        if (hour24 == 0 || hour24 == 12)
        {
            return 12;
        }
        int newNum = Convert.ToInt32(hour24);
        // For any other hour, we get the remainder when divided by 12.
        return newNum % 12;
    }
}
