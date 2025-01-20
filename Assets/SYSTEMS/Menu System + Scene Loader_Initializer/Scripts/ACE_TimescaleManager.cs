using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACE_TimescaleManager : MonoBehaviour
{
    public void SetTimescale(float newTimescale)
    {
        Time.timeScale = newTimescale;
    }
}
