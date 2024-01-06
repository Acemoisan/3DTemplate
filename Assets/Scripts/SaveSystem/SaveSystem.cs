/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "Scriptable Objects/Save Manager/Save System")]
public class SaveSystem : ScriptableObject
{
    [Header("Dependencies")]
    public ItemDatabase _itemDatabase;
    [SerializeField] TimeManagerSO timeManagerSO;
    //public VideoClipSO introCinematicClip;


    [Header("Strings")]
    [SerializeField] string worldFileFolderName = "World Files";
    [SerializeField] string individualWorldFileName = "worldData.game";



    public SaveData saveData = new SaveData();
    public SaveFileSO savedFiles = new SaveFileSO();
    [HideInInspector] public SceneData sceneData = new SceneData();
    [HideInInspector] public string iconString;
    [HideInInspector] public string playerLoadingName; //THIS IS FILLED FROM THE CHARACTER SLOT SCRIPT/BUTTON. IT IS PASSED TO THE PLAYER UPON LOADING THE GAME
    [SerializeField] string selectedWorldString; //THIS IS FILLED WHEN LOADING GAME DATA OR CREATING A NEW GAME. THIS STAYS FOR THE DURATION OF THE GAME. IF THE PLAYER WANTS TO DELET THE FILE. THIS NAME IS USED.
    
    
    



    public void ClearProfileID() //CLEARED ON MENU INITIALIZER SO THAT WHEN LOADING, A PREVIOUS NAME IS NOT USED
    {
        selectedWorldString = "";
        playerLoadingName = "";
        iconString = "";
    }





    #region ----------- SAVE / LOAD TO DISK -------------

    ///////////////////////////////////////////
    //LOADING DATA FROM THE DISK // FILEMANAGER
    ///////////////////////////////////////////
    void LoadWorldFromDisk(string worldName)
    {
        string worldNamePath = worldFileFolderName + "/" + worldName;

        // TRY TO FIND FILE
        if (FileManager.LoadFromFile(individualWorldFileName, worldNamePath, out var json)) 
        {
            this.saveData.FromJson(json);
            Debug.Log("World data loaded: " + Application.persistentDataPath + "/" + worldNamePath);
            return;
        }
        // IF FILE NOT FOUND
        else
        {
            this.saveData.ApplyDefaultData(); //ADD DEFAULT DATA
            this.SaveDataToDisk(); //SAVING DATA
            this.LoadWorldFromDisk(worldName); //LOADING THE DATA JUST CREATED
            Debug.Log("World data not found. Creating new data: " + Application.persistentDataPath + "/" + worldNamePath);
        }
    }

    // public void LoadSceneDataFromDisk(int sceneindex) // this is public because it is called using events
    // {
    //     // TRY TO FIND FILE. WITH SCENE DATA
    //     if (FileManager.LoadFromFile(sceneindex + ".sceneData", selectedFileString, out var json))//"Scene" + ".sceneData", selectedFileString, out var json)) 
    //     {
    //         this.sceneData.FromJson(json);


    //         // foreach (IPersistant persistant in FindAllPersistantObjects())
    //         // {
    //         //     persistant.LoadData(this.saveData, this.sceneData);
    //         //     Debug.Log("Loading scene data " + persistant);
    //         // }
    //     }
    //     // IF FILE NOT FOUND
    //     else
    //     {
    //         this.sceneData.ResetData();
    //         this.SaveDataFromSceneToDisk(sceneindex);
    //         this.LoadSceneDataFromDisk(sceneindex);
    //         Debug.Log("Scene data not found. Creating new data");
    //     }


    // }




    ///////////////////////////////////////////
    //SAVING DATA TO THE DISK // FILEMANAGER
    ///////////////////////////////////////////
    void SaveDataToDisk()
    {
        // if (FileManager.MoveFile(saveFiles[0].saveFileName, saveFiles[0].backupSaveFileName)) //REMOVING BACK UP FILE TEMPORARILY
        // {

        // }

        string worldNamePath = worldFileFolderName + "/" + selectedWorldString;
        if (FileManager.WriteToFile(individualWorldFileName, worldNamePath, saveData.ToJson())) //CREATING A FILE AND PASSING ITS CONTENTS
        {
            Debug.Log("Save successful!");
        }
    }

    // void SaveDataFromSceneToDisk(int sceneNumber)
    // {
    //     if (FileManager.WriteToFile(sceneNumber + ".sceneData", selectedFileString, sceneData.ToJson()))//sceneNumber + ".sceneData", selectedFileString, sceneData.ToJson()))
    //     {
    //         Debug.Log("Save successful!");
    //     }
    // }

    void SaveGameSaveSlotsToDisk()
    {
        if (FileManager.WriteToFile("savedFiles.files", worldFileFolderName, savedFiles.ToJson())) //CREATING A FILE AND PASSING ITS CONTENTS
        {
            Debug.Log("Saved Game Worlds successfully! " + savedFiles.ToJson());
        }
    }

    public void LoadGameFilesFolder()
    {
        // FIRST TRIES TO FIND FILE WITH THE SAVED NAME. PULLS DATA
        if (FileManager.LoadFromFile("savedFiles.files", worldFileFolderName, out var json)) 
        {
            this.savedFiles.FromJson(json);
            return;
        }
        // IF FILE NOT FOUND
        else
        {
            if (FileManager.WriteToFile("savedFiles.files", worldFileFolderName, savedFiles.ToJson())) //CREATING A FILE AND PASSING ITS CONTENTS
            {
                LoadGameFilesFolder();
            }
        }
    }
    #endregion





    #region ----------- PUBLIC FUNCTIONS FOR SAVING / LOADING / DELETING FILES -------------

    ///////////////////////////////////////////
    //PUBLIC FUNCTIONS FOR SAVING / LOADING / DELETING FILES
    ///////////////////////////////////////////
    public void LoadGame(string profileID)
    {
        //SELECTING WHICH FILE TO LOAD BASED ON THE PROFILE NAME GIVEN
        selectedWorldString = profileID;

        LoadWorldFromDisk(selectedWorldString);

        Debug.Log(this.saveData.hour + ":" + this.saveData.minute);
        LoadGlobalData();
    }

    public void SaveGame(PlayerAttributes playerAttributes, PlayerInventory _playerInventory)
    {
        //SAVING VARIOUS DATA
        //SavePlayerData(playerAttributes, _playerInventory);

        SaveGlobalData();

        //Saving the data to a file
        this.SaveDataToDisk();
    }

    public void SaveGame()
    {
        SaveGlobalData();

        //Saving the data to a file
        this.SaveDataToDisk();        
    }

    public void DeleteFile() //used in game 
    {
        FileManager.DeleteFile("data.game", selectedWorldString);

        this.SaveGameSaveSlotsToDisk();
    }

    public void DeleteAllFiles()
    {
        savedFiles.ClearAllfiles();
    }
    #endregion





    #region ----------- LOADING GAME FILES -------------
    //returning data to the save slots
    public void GetWorldDataForSaveSlot(string profileID, out SaveData file) 
    {
        string worldNamePath = worldFileFolderName + "/" + profileID;
        //var fullPath = Path.Combine(Application.persistentDataPath, worldNamePath, individualWorldFileName);
        if (FileManager.LoadFromFile(individualWorldFileName, worldNamePath, out var json)) 
        {
            file = new SaveData();
            file.FromJson(json);
        }
        else 
        {
            Debug.Log("Save Slot could not find world: " + worldNamePath);
            file = null;
        }
    }
    #endregion





    #region ----------- FUNCTIONS WHEN CREATING A NEW FILE -------------

        ///////////////////////////////////////////
        //FUNCTIONS WHEN CREATING A NEW FILE
        ///////////////////////////////////////////
        public void StartNewFile(string customName) 
        {
            //savedFiles is initialized above. At LoadGameFilesFolder()
            if(savedFiles.files.Contains(customName) == false) 
            {
                savedFiles.files.Add(customName);
            }
            
            SaveGameSaveSlotsToDisk();

            //Remove Later
            LoadGame(customName);
        }

        // public void SaveIconString(string iconObjectName)
        // {
        //     iconString = iconObjectName;
        // }

        // public void StartNewGame()
        // {
        //     LoadGame(selectedWorldString);
        // }
    #endregion





    #region ----------- SAVE DATA -------------
    // public void SavePlayerData(PlayerAttributes _playerAttributes, PlayerInventory _playerInventory) 
    // {
    //     // if (hostPlayer)
    //     // {
    //     //     SaveGlobalData();
    //     // }
    //     SaveIndividualPlayerData(_playerAttributes, _playerInventory);
    // }

    public void SaveGlobalData()
    {
        //SavePlayerNames();
        SaveTime();
        SaveRealTime();
        SaveWorldAttributes();
        //SaveCinematicState();
        //SaveAllNPCs();
    }

    // public void SaveSceneData(List<IPersistant> saveableItems) //saved upon leaving each scene. AND SAVING GAME
    // {
    //     foreach (IPersistant persistant in saveableItems)
    //     {
    //         persistant.SaveData(ref saveData, ref sceneData);
    //     }
    //     this.SaveDataFromSceneToDisk(SceneManager.GetActiveScene().buildIndex);
    // }


    // void SaveIndividualPlayerData(PlayerAttributes _playerAttributes, PlayerInventory _playerInventory)
    // {
    //     foreach(var player in this.saveData.players)
    //     {
    //         if (player.playerName == _playerAttributes.GetPlayerName())
    //         {
    //             this.saveData.players.Remove(player);
    //         }
    //     }

    //     this.saveData.players.Add(new IndividualPlayerData(_playerAttributes.GetPlayerName()));

    //     SavePlayerAttributes(_playerAttributes, _playerAttributes.GetPlayerName());
    //     SaveInventory(_playerAttributes.GetPlayerName(), _playerInventory);
    // }





    //////////////////////////
    //GLOBAL
    //////////////////////////
    void SaveWorldAttributes() 
    {
        this.saveData.worldName = selectedWorldString;

        // if(this.saveData.iconString == "") 
        // {
        //     this.saveData.iconString = iconString;
        // }
    }


    public void SaveTime() 
    {
        //this.saveData.daysPlaying = TimeManager.instance.daysPlaying;
        this.saveData.hour = TimeManager.Instance.hour;
        this.saveData.minute = TimeManager.Instance.minute;
        this.saveData.second = TimeManager.Instance.second;
        this.saveData.secondsPerInGameTenMinutes = TimeManager.Instance.secondsPerInGameTenMinutes;
        this.saveData.monthIndex = TimeManager.Instance.monthIndex;
        this.saveData.dayOfTheMonthIndex = TimeManager.Instance.dayOfTheMonthIndex;
        this.saveData.dayOfTheWeekIndex = TimeManager.Instance.dayOfTheWeekIndex;
        this.saveData.dayOfTheWeekString = TimeManager.Instance.dayOfTheWeekString;
    }

    void SaveRealTime()
    {
        Debug.Log("Saving real time");
        Debug.Log(TimeManager.Instance.timeSaved);
        Debug.Log(TimeManager.Instance.dateSaved);
        this.saveData.realMinutes = TimeManager.Instance.realMinutes;
        this.saveData.realHours = TimeManager.Instance.realHours;
        this.saveData.timeSaved = TimeManager.Instance.timeSaved;
        this.saveData.dateSaved = TimeManager.Instance.dateSaved;
    }

    // void SaveCinematicState()
    // {
    //     this.saveData.introCinematicState = introCinematicClip.alreadyPlayed;
    // }



    //////////////////////////
    //INDIVIDUAL
    //////////////////////////
    void SavePlayerAttributes(PlayerAttributes _playerAttributes, string _playerName) 
    {
           
        foreach(var player in this.saveData.players)
        {
            if (player.playerName == _playerName)
            {
                player.playerHealth = _playerAttributes.GetPlayerHealth();
                player.playerEnergy = _playerAttributes.GetPlayerEnergy();
            }
        }
    }

    void SaveInventory(string _playerName, PlayerInventory _playerInventory)
    {
        foreach(var player in this.saveData.players)
        {

            if (player.playerName == _playerName)
            {                
                //before i save the player inventory data. I need to clear them 
                player.mainInventoryItems.Clear();
                player.hotbarInventoryItems.Clear();

                //SAVE GOLD
                player.gold = _playerInventory.GetPlayerCurrencyValue();


                //Items
                //Save all items. regardless if null or full. This saves a full list. 
                foreach(var itemSlot in _playerInventory.inventorySlots)
                {
                    if (itemSlot.item != null) 
                    {
                        player.mainInventoryItems.Add(new SerializedItem(itemSlot.item.GetItemName(), itemSlot.count, itemSlot.item.GetItemLevel()));
                    }
                    else 
                    {
                        player.mainInventoryItems.Add(new SerializedItem("empty", 0));
                    }
                }

                foreach(var itemSlot in _playerInventory.hotBarSlots)
                {
                    if (itemSlot.item != null) 
                    {
                        player.hotbarInventoryItems.Add(new SerializedItem(itemSlot.item.GetItemName(), itemSlot.count, itemSlot.item.GetItemLevel()));
                    }
                    else 
                    {
                        player.hotbarInventoryItems.Add(new SerializedItem("empty", 0));
                    }
                }

                foreach(var itemSlot in _playerInventory.miscSlots)
                {
                    if (itemSlot.item != null) 
                    {
                        player.miscInventoryItems.Add(new SerializedItem(itemSlot.item.GetItemName(), itemSlot.count, itemSlot.item.GetItemLevel()));
                    }
                    else 
                    {
                        player.miscInventoryItems.Add(new SerializedItem("empty", 0));
                    }
                }
            }
        }
    }

    void SaveCurrency()
    {

    }
    #endregion





    #region ----------- LOAD DATA -------------

    public void LoadGlobalData() //most of these objects need to be loaded first. and then visualized in game. With an invoke delay 
    {
        LoadTime(); //delayed
        LoadRealTime();
        LoadWorldAttributes();
        //LoadAllNPCs(); //delayed
        //LoadCinematicState();
    }

    // public void LoadActiveScene(int sceneIndex) //loaded upon level initializer game object 
    // {
    //     LoadSceneDataFromDisk(sceneIndex);   
    // }

    // public void LoadSceneData(List<IPersistant> saveableItems, int sceneIndex)
    // {
    //     LoadSceneDataFromDisk(sceneIndex);  
    //     foreach (IPersistant persistant in saveableItems)
    //     {
    //         persistant.LoadData(this.saveData, this.sceneData);
    //     }
    // }



    //////////////////////////
    //GLOBAL
    //////////////////////////
    void LoadWorldAttributes()
    {
        //selectedWorldString = saveData.worldName;
    }

    void LoadTime() 
    {
        //TimeManager.instance.daysPlaying = this.saveData.daysPlaying;
        // TimeManager.instance.hour = this.saveData.hour;
        // TimeManager.instance.minute = this.saveData.minute;
        // TimeManager.instance.second = this.saveData.second;
        // TimeManager.instance.secondsPerInGameTenMinutes = this.saveData.secondsPerInGameTenMinutes;
        // TimeManager.instance.monthIndex = this.saveData.monthIndex;
        // TimeManager.instance.dayOfTheMonthIndex = this.saveData.dayOfTheMonthIndex;
        // TimeManager.instance.dayOfTheWeekIndex = this.saveData.dayOfTheWeekIndex;
        // TimeManager.instance.dayOfTheWeekString = this.saveData.dayOfTheWeekString;
        timeManagerSO.ManuallySetTime(this.saveData.hour, this.saveData.minute);
        timeManagerSO.ManuallySetDay(this.saveData.monthIndex, this.saveData.dayOfTheMonthIndex, this.saveData.year);
        Debug.Log("Loading time");
    }

    void LoadRealTime()
    {
        TimeManager.Instance.realMinutes = this.saveData.realMinutes;
        TimeManager.Instance.realHours = this.saveData.realHours;
        TimeManager.Instance.dateSaved = this.saveData.dateSaved;
        TimeManager.Instance.timeSaved = this.saveData.timeSaved;
    }

    // void LoadCinematicState()
    // {
    //     introCinematicClip.alreadyPlayed = this.saveData.introCinematicState;
    // }   





    //////////////////////////
    //INDIVIDUAL PLAYER
    //////////////////////////
    public void LoadPlayerAttributes(PlayerAttributes _playerAttributes) 
    {
        foreach(var player in this.saveData.players)
        {
            if (player.playerName == _playerAttributes.GetPlayerName())
            {
                _playerAttributes.SetPlayerName(player.playerName);
                _playerAttributes.SetPlayerHealth(player.playerHealth);
                _playerAttributes.SetPlayerEnergy(player.playerEnergy);
            }
        }       
        //_playerAttributes._m = this.saveData.playerMaxHealth;
        //_playerAttributes._entityMaxEnergy = this.saveData.playerMaxEnergy;
        // _playerAttributes._entityMaxMana = this.saveData.playerMaxMana;
    }

    public void LoadPlayerInventory(PlayerInventory _playerInventory, string _playerName) 
    {
        foreach(var player in this.saveData.players)
        {
            if (player.playerName == _playerName)
            {
                //clearing all PLAYER slots before re loading them. 
                _playerInventory.inventorySlots.Clear();
                _playerInventory.hotBarSlots.Clear();
                _playerInventory.miscSlots.Clear();


                //LOAD GOLD
                _playerInventory.SetCurrency(player.gold);


                for (int itemSlot = 0; itemSlot < 21; itemSlot++)
                {
                    _playerInventory.inventorySlots.Add(new ItemSlot());
                }
                for (int itemSlot = 0; itemSlot < 10; itemSlot++)
                {
                    _playerInventory.hotBarSlots.Add(new ItemSlot());
                }
                for (int itemSlot = 0; itemSlot < 4; itemSlot++)
                {
                    _playerInventory.miscSlots.Add(new ItemSlot());
                }




                //for each item we have in the MAIN INVENTORY LIST save data file...
                for (int itemIndex = 0; itemIndex < player.mainInventoryItems.Count; itemIndex++)
                {

                    //Get the serialized data (the saved item info)
                    var serializedItem = player.mainInventoryItems[itemIndex];

                    //Get the actual scriptable objects from the one in the resource folder
                    var itemSO = _itemDatabase.items.Find((c) => { return c.GetItemName() == serializedItem.itemName; }); 

                    //if the itemSO is actually there
                    if (itemSO != null)// && !playerItems.Contains(itemSO))
                    {
                        _playerInventory.inventorySlots[itemIndex].item = itemSO;
                        _playerInventory.inventorySlots[itemIndex].count = serializedItem.count;
                        itemSO.SetItemLevel(serializedItem.itemLevel);
                    }
                    else 
                    {
                        _playerInventory.inventorySlots[itemIndex].Clear();
                        Debug.Log("Item Slot empty in saved data. Or Item: " + serializedItem.itemName + " not found in database");
                    }
                }

                //for each item we have in the HOTBAR LIST save data file...
                for (int itemIndex = 0; itemIndex < player.hotbarInventoryItems.Count; itemIndex++)
                {

                    //Get the serialized data (the saved item info)
                    var serializedItem = player.hotbarInventoryItems[itemIndex];

                    //Get the actual scriptable objects from the one in the resource folder
                    var itemSO = _itemDatabase.items.Find((c) => { return c.GetItemName() == serializedItem.itemName; }); 

                    //if the itemSO is actually there
                    if (itemSO != null)// && !playerItems.Contains(itemSO))
                    {
                        _playerInventory.hotBarSlots[itemIndex].item = itemSO;
                        _playerInventory.hotBarSlots[itemIndex].count = serializedItem.count;
                        itemSO.SetItemLevel(serializedItem.itemLevel);

                    }
                    else 
                    {
                        _playerInventory.hotBarSlots[itemIndex].Clear();
                        Debug.Log("Item Slot empty in saved data. Or Item: " + serializedItem.itemName + " not found in database");
                    }
                }

                //for each item we have in the POTION LIST save data file...
                for (int itemIndex = 0; itemIndex < player.miscInventoryItems.Count; itemIndex++)
                {

                    //Get the serialized data (the saved item info)
                    var serializedItem = player.miscInventoryItems[itemIndex];

                    //Get the actual scriptable objects from the one in the resource folder
                    var itemSO = _itemDatabase.items.Find((c) => { return c.GetItemName() == serializedItem.itemName; }); 

                    //if the itemSO is actually there
                    if (itemSO != null)// && !playerItems.Contains(itemSO))
                    {
                        _playerInventory.miscSlots[itemIndex].item = itemSO;
                        _playerInventory.miscSlots[itemIndex].count = serializedItem.count;
                        itemSO.SetItemLevel(serializedItem.itemLevel);

                    }
                    else 
                    {
                        _playerInventory.miscSlots[itemIndex].Clear();
                    }
                }
            }
        }
    }

    #endregion





    #region ----------- GET -------------
    public string GetFileName()
    {
        return selectedWorldString;
    }
    #endregion

}