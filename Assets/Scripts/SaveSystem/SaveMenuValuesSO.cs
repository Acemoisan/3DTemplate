using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveMenuValues", menuName = "Scriptable Objects/Values/SaveMenuValues")]
public class SaveMenuValuesSO : ScriptableObject
{
    public string fileNameText;
    public string season;
    public string day;
    public string year;
    public string gold;
    public string timePlayed;
    public string dateSaved;
    public string timeSaved;
}
