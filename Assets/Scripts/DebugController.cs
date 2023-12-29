using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using QFSW.QC;
using QFSW.QC.Suggestors.Tags;
using System.Linq;

public class DebugController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Transform playerEntity;
    [SerializeField] ConsoleStats consoleStats;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] CameraModeController cameraModeController;
    //[SerializeField] SceneLoader sceneLoader;
    [SerializeField] TimeManagerSO currentTime;
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] ItemSpawnerSO itemSpawner;




    [Header("Player Inventory Kit")]
    [SerializeField] List<ItemSlot> inventoryKit;


    //private
    public static List<string> itemNames;



    void Start()
    {
        itemNames = itemDatabase.items.Select(item => item.name).ToList();
    }


    void Update()
    {
        consoleStats.UpdatePlayerStats(playerEntity, playerInventory, playerAttributes, playerController, playerAnimation, cameraModeController, playerInteraction);
        consoleStats.UpdateWorldStats(currentTime);
    }




    #region QUICK COMMANDS

    //TIME COMMANDS
    ///////////////
    [Command("time_next_day", "Skips to next morning. 6AM")]
    public void NextMorning()
    {
        currentTime.SkipToNextMorning();
    }

    [Command("time_pause", "Pauses Time")]
    public void PauseTime(bool pause)
    {
        currentTime.PauseTime(pause);
    }

    [Command("time_set", "Sets Hour of Day (24HR)")]
    public void SetTime(int time)
    {
        currentTime.SetTime(time);
    }

    [Command("time_set_seconds_per_10_min", "Sets Time Increment - (Seconds per in game 10 min) Default - 8")]
    public void SetTimeIncrement(int increment)
    {
        currentTime.SetTimeIncrement(increment);
    }



    //PLAYER COMMANDS
    /////////////////
    [Command("player_set_camera_mode", "Sets Player Camera Mode")]
    public void SetPlayerCameraMode(CameraModes cameraMode)
    {
        cameraModeController.SetCameraMode(cameraMode);
    }
    
    //GIVE COMMANDS
    [Command("player_give_gold", "Increases Player Gold")]
    public void GivePlayerGold(int goldCount)
    {
        playerInventory.AddCurrency(goldCount);
    }

    [Command("player_give_item", "Gives Player Item")]
    public void GivePlayerItem([ItemName]string itemName, int count)
    {
        ItemSO itemToSpawn;

        itemToSpawn = itemDatabase.GetMatchingItem(itemName);
        playerInventory.AddItem(itemToSpawn, count);
    }

    [Command("player_give_inventory_kit", "Gives Player PreMade Inventory Kit")]
    public void GiveInventoryKit()
    {
        foreach(ItemSlot slot in inventoryKit)
        {
            playerInventory.AddItem(slot.item, slot.count);
        }
    }

    [Command("player_clear_inventory", "Clears Entire Player Inventory and Hotbar")]
    public void ClearPlayerInventory()
    {
        playerInventory.ClearInventory();
    }



    //STAT COMMANDS
    [Command("player_increase_health", "Increases Player Health")]
    public void IncreaseHealth(float value)
    {
        playerAttributes.IncreaseHealth(value);
    }

    [Command("player_increase_energy", "Increases Player Energy")]
    public void IncreaseEnergy(float value)
    {
        playerAttributes.IncreaseEnergy(value);
    }

    [Command("player_refresh_stats", "Resfreshes Players Stats. To Current Max Values.")]
    public void RefreshPlayerStats()
    {
        playerAttributes.RefreshStats();
    } 

    [Command("player_revert_stats", "Reverts Players Stats. To Original Values.")]
    public void RevertPlayerStats()
    {
        playerAttributes.RevertStats();
        playerController.RevertStats();
    } 

    [Command("player_kill", "Kills Player. Plays Death Event")]
    public void KILLPlayer()
    {
        playerAttributes.Kill();
    }  

    [Command("player_set_speed", "Sets Player Speed")]
    public void SetPlayerSpeed(float value)
    {
        playerController.SetMoveSpeed(value);
    }

    [Command("player_set_jump_height", "Sets Player Jump Height")]
    public void SetPlayerJumpHeight(float value)
    {
        playerController.SetJumpHeight(value);
    }

    [Command("player_set_gravity", "Sets Player Gravity")]
    public void SetPlayerGravity(float value)
    {
        playerController.SetGravity(value);
    }




    //MIC COMMANDS
    //////////////
    public void SpawnItemAtPosition(string itemName, int count, Vector3 position)
    {
        ItemSO itemToSpawn = itemDatabase.GetMatchingItem(itemName);
        itemSpawner.SpawnItem(itemToSpawn, position, count);
    }

    public void COM_PrintAllItemNamesToTextFile()
    {
        // string itemListString = "";
        // foreach (ItemSO item in itemDatabase.items)
        // {
        //     itemListString += item.GetItemName() + "\n";
        // }

        // ToTextFile.CreateNewTextFile("Databases/Item Names", itemListString, true);
    }
    #endregion
}
