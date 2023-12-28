using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using QFSW.QC;

public class DebugController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] PlayerController playerController;
    //[SerializeField] SceneLoader sceneLoader;
    [SerializeField] TimeManagerSO currentTime;
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] ItemSpawnerSO itemSpawner;




    [Header("Text References")]
    [SerializeField] TextMeshProUGUI healthValueRef;
    [SerializeField] TextMeshProUGUI energyValueRef;
    [SerializeField] TextMeshProUGUI speedValueRef;
    [SerializeField] TextMeshProUGUI inGameDayValueRef;
    [SerializeField] TextMeshProUGUI inGameTimeValueRef;
    [SerializeField] TextMeshProUGUI inGameSecondsPerMinuteValueRef;



    [Header("Player Inventory Kit")]
    [SerializeField] List<ItemSlot> inventoryKit;







    #region QUICK COMMANDS

    //TIME COMMANDS
    ///////////////
    [Command("time_next_morning", "Skips to next morning. 6AM")]
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

    [Command("time_set_inc", "Sets Time Increment - (Seconds per in game 10 min) Default - 8")]
    public void SetTimeIncrement(int increment)
    {
        currentTime.SetTimeIncrement(increment);
    }



    //PLAYER COMMANDS
    /////////////////
    
    //GIVE COMMANDS
    [Command("player_give_gold", "Increases Player Gold")]
    public void GivePlayerGold(int goldCount)
    {
        playerInventory.AddCurrency(goldCount);
    }

    [Command("player_give_item", "Gives Player Item")]
    public void GivePlayerItem(string itemName, int count)
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
