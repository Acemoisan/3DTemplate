/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class IndividualPlayerData
{
    public IndividualPlayerData(string _playerName)
    {
        //ResetPlayerData();
        playerName = _playerName;
    }

    
    //INDIVIDUAL PLAYER DATA
    ////////////////////////
    ////////////////////////
    public string playerName;
    //public Sprite playerIcon;
    // public int playerMaxLevel;
    // public float playerMaxHealth;
    // public float playerMaxMana;
    // public float playerMaxEnergy;
    
    public float playerHealth;
    public float playerEnergy;
    public int gold;

    //STATS
    // public int stepsWalked;
    // public int numberOfDeaths;
    // public int minutesPlayed;
    

    //POSITIONS
    // public float playerPosX;
    // public float playerPosY;



    //INVENTORY
    public List<SerializedItem> mainInventoryItems = new List<SerializedItem>();
    public List<SerializedItem> hotbarInventoryItems = new List<SerializedItem>();
    public List<SerializedItem> miscInventoryItems = new List<SerializedItem>();



    public string divider = "==============================================================";






    public void ResetPlayerData()
    {
        //PLAYERATTRIBUTES
        this.playerName = "";

        //current
        this.playerHealth = 1000;
        this.playerEnergy = 500;




        //Pet Attributes

        this.mainInventoryItems.Clear();

        this.hotbarInventoryItems.Clear();
        SetDefaultHotbarItems();

        this.miscInventoryItems.Clear();
    }

    public string GetPlayerName() 
    {
        return playerName;
    }

    void SetDefaultHotbarItems()
    {
        // var hoeTool = Resources.Load<ItemSO>("Inventory/Tools/Hoe");
        // if (hoeTool != null)
        // {
        //     this.hotbarInventoryItems.Add(new SerializedItem(hoeTool.itemName, 1));
        // }
        // else 
        // {
        //     Debug.Log("Hoe Tool not in Resources Folder");
        // }
    }
}



[Serializable]
public class SaveData
{

    ///////////////////////////////////////////////   
    //ADD THE DATA I WANT TO BE SAVED IN THE FILE
    ///////////////////////////////////////////////    
    public Guid UUID;
    //public float distanceWalked = 0;

    //WORLD ATTRIBUTES
    public string worldName;
    public string iconString;


    //TIME
    public int hour;
    public int minute;
    public int second;
    public int secondsPerInGameTenMinutes;
    public int dayOfTheMonthIndex;
    public int dayOfTheWeekIndex;
    public string dayOfTheWeekString;
    public int year;
    public int monthIndex;
    public int realMinutes;
    public int realHours;
    public string dateSaved;
    public string timeSaved;




    //CINEMATICS
    public bool introCinematicState;



    //EVENT DATA
    ////////////
    //public bool spawnedInHouse = false;


    public string divider = "==============================================================";
    public List<IndividualPlayerData> players;


    public void ApplyDefaultData()
    {
        this.ResetData();
    }


    public void ResetData()
    {

        //world attributes 
        this.worldName = "New Save";
        //this.iconString = "";


        //TIME
        //this.daysPlaying = 1;
        this.hour = 12;
        this.minute = 0;
        this.second = 0;
        this.secondsPerInGameTenMinutes = 8;
        this.monthIndex = 1;
        this.dayOfTheMonthIndex = 1;
        this.dayOfTheWeekIndex = 0;
        this.dayOfTheWeekString = "Mon.";
        this.realMinutes = 0;
        this.realHours = 0;
        // this.dateSaved = "";
        // this.timeSaved = "";
        this.year = 1;



        //CINEMATICS
        this.introCinematicState = false;
        

        players.Clear();

        //Scene Data
        //spawnedInHouse = false;
    }


    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public string GetProfileID() 
    {
        return worldName;
    }

    // public int GetDaysPlayed() 
    // {
    //     return daysPlaying;
    // }

}

[System.Serializable]
public class SceneData
{
    //ResourceNodes
    //public List<SerializedResourceNode> _farmRockResourceNodes = new List<SerializedResourceNode>();
    //public List<SerializedPlaceableObject> placeableObjects = new List<SerializedPlaceableObject>();
    //public List<SerializedAnimal> farmAnimals = new List<SerializedAnimal>();
    //public List<SerializedContainer> _containers = new List<SerializedContainer>();
    //public List<SerializedPosition> serializedPositions = new List<SerializedPosition>();




    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public void ResetData()
    {

    }
}


