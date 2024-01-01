/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "New Save File", menuName = "Scriptable Objects/Save Manager/Save File")]
public class SaveFileSO
{
    //collect various SO's of objects I want in each save file
    // public string saveFileName = "save_data.dat";
    // public string backupSaveFileName = "save_data.dat.bak";
    // public int index = 0; //savged file index

    public List<string> files;



    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
