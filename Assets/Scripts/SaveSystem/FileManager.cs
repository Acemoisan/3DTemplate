/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileManager
{



    ///////////////////////////////////////////////   
    //USED TO WRITE A FILENAME AND GIVE IT CONTENTS
    ///////////////////////////////////////////////
    // public static bool WriteToFile(string fileName, string fileContents) 
    // {
    //     var fullPath = Path.Combine(Application.persistentDataPath, fileName);

    //     try
    //     {
    //         File.WriteAllText(fullPath, fileContents);
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Failed to write to {fullPath} with exception {e}");
    //         return false;
    //     }
    // }

    public static bool WriteToFile(string folderName, string fileName, string fileContents) 
    {
        var fullPath = Path.Combine(Application.persistentDataPath, folderName, fileName);


        try
        {
            if (!Directory.Exists(fullPath)) {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            File.WriteAllText(fullPath, fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    

    }



    ///////////////////////////////////////////////   
    //USED TO READ A FILENAME AND RETURN ITS CONTENTS
    ///////////////////////////////////////////////
    public static bool LoadFromFile(string folderName, string fileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        Debug.Log($"'Loading <color=#78D09F>{folderName}/{fileName}</color> From: <color=#EFB628>{fullPath}</color>");

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result = "";
            return false;
        }
    }


    public static void DeleteFile(string fileName, string profileID)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, profileID, fileName);
        Debug.Log(fullPath);
        if (fullPath != null)
        {
            File.Delete(fullPath); //first imust delete files inside directory
            Directory.Delete(Path.Combine(Application.persistentDataPath, profileID)); //and then i can delete directory

        }
        else 
        {
            Debug.Log("file not found");
        }

    }



    ///////////////////////////////////////////////   
    //LOAD ALL PROFILES
    ///////////////////////////////////////////////
    // public static Dictionary<string, SaveData> LoadAllProfiles()
    // {
    //     Dictionary<string, SaveData> profileDictionary = new Dictionary<string, SaveData>();

    //     //loop over all directory names in the data directory path
    //     IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(Application.persistentDataPath).EnumerateDirectories();
    //     foreach (DirectoryInfo dirInfo in dirInfos)
    //     {
    //         string profileID = dirInfo.Name;

    //         string fullPath = Path.Combine(Application.persistentDataPath, profileID, "data.txt");
    //         if (!File.Exists(fullPath))
    //         {
    //             Debug.LogError($"skipping profile because it does not contain data " + profileID);
    //             continue;
    //         }

    //         SaveData profileData = load()
    //     }

    //     return profileDictionary;
    // }


    ///////////////////////////////////////////////   
    //CREATES A BACKUP OF A FILE
    ///////////////////////////////////////////////
    public static bool MoveFile(string fileName, string newFileName)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);
        var newFullPath = Path.Combine(Application.persistentDataPath, newFileName);

        try
        {
            if (File.Exists(newFullPath))
            {
                File.Delete(newFullPath);
            }
            File.Move(fullPath, newFullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to move file from {fullPath} to {newFullPath} with exception {e}");
            return false;
        }
    }

//     public static bool TempIsFileSaved() //checking if string file exists or not
//     {
//             return File.Exists(Application.persistentDataPath + "...");
//     }

//     public static void TempSaveFile(InventorySO inventory) //THIS MAKES A SAVE WITH FILE NAME UNDER > USERS > ??? > APPDATA ? LOCALLOW > OMOHU > FIREFLYCOVE > SAVEFILENAME0 
//     {
//         //File.WriteAllText(Application.persistentDataPath + "SaveOne");
//         Directory.CreateDirectory(Application.persistentDataPath + "..."); 
//         if(!Directory.Exists(Application.persistentDataPath + "/.../character_Inventory"))
//         {
//             Directory.CreateDirectory(Application.persistentDataPath + "/.../character_Inventory"); 
//         }
//         BinaryFormatter bf = new BinaryFormatter();
//         FileStream file = File.Create(Application.persistentDataPath + "/.../character_Inventory/character_Inventory_Save.txt");
//         var json = JsonUtility.ToJson(inventory);
//         bf.Serialize(file, json);
//         file.Close();
//     }
 }
