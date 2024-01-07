using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using QFSW.QC;
using QFSW.QC.Suggestors.Tags;
using System.Linq;


[System.Serializable]
public enum CanvasSize
{
    Small,
    Medium,
    Large
}

public class DebugController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Transform playerEntity;
    [SerializeField] GameObject playerArmature;
    [SerializeField] ConsoleStats consoleStats;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerEffectsHandler playerEffectsHandler;
    [SerializeField] CameraModeController cameraModeController;
    [SerializeField] SetHudSize setHudSize;
    [SerializeField] HUDUI hudUI;
    //[SerializeField] SceneLoader sceneLoader;
    [SerializeField] TimeManagerSO currentTime;
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] EntityDatabase entityDatabase;
    [SerializeField] ItemSpawnerSO itemSpawner;
    [SerializeField] EntitySpawnerSO entitySpawner;




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
        consoleStats.UpdatePlayerStats(playerEntity, playerInventory, playerAttributes, playerController, playerAnimation, cameraModeController, playerInteraction, playerEffectsHandler);
        consoleStats.UpdateWorldStats(currentTime);
    }




    #region QUICK COMMANDS
    [Command("gamemode", "Set Game Mode")]
    public void SetGameMode(GameMode gameMode = GameMode.Survival)
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.ChangeGameMode(gameMode);
    }

    [Command("hud_size", "Set HUD size")]
    public void SetHudSize(CanvasSize canvasSize = CanvasSize.Medium)
    {
        if(setHudSize == null) return;
        setHudSize.SetSize(canvasSize);
    }

    [Command("hud_toggle", "Toggle HUD")]
    public void ToggleHud(bool toggle)
    {
        if(hudUI == null) return;
        if(toggle)
        {
            hudUI.SetHUDVisibility(1);
            return;
        }
        else
        {
            hudUI.SetHUDVisibility(0);
            return;
        }
    }



    //DEBUG COMMANDS
    ////////////////
    [Command("debug_draw_aim_rays", "Draws Aim Rays")]
    public void DrawAimRays(bool drawAimRays)
    {
        if(DebugManager.Instance == null) return;
        DebugManager.Instance.SetDebugDrawAimRays(drawAimRays);
    }



    //TIME COMMANDS
    ///////////////
    [Command("time_next_day", "Skips to next morning. 6AM")]
    public void NextMorning()
    {
        currentTime.SkipToNextMorning();
    }

    [Command("time_set_day", "MM DD YYYY -Sets Day of Month")]
    public void SetDay(int monthIndex, int dayOfTheMonthIndex, int yearIndex)
    {
        currentTime.ManuallySetDay(monthIndex, dayOfTheMonthIndex, yearIndex);
    }

    [Command("time_pause", "Pauses Time")]
    public void PauseTime(bool pause)
    {
        currentTime.PauseTime(pause);
    }

    [Command("time_set", "Sets Hour of Day (24HR)")]
    public void SetTime(int hour, int minute = 0)
    {
        currentTime.ManuallySetTime(hour, minute);
    }

    [Command("time_set_seconds_per_10_min", "Sets Time Increment - (Seconds per in game 10 min) Default - 8")]
    public void SetTimeIncrement(int increment)
    {
        currentTime.SetTimeIncrement(increment);
    }

    [Command("time_set_scale", "Sets Time Scale")]
    public void SetTimeScale(float timeScale = 1)
    {
        currentTime.SetTimeScale(timeScale);
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
    [Command("player_set_effect", "Sets Player Effect")]
    public void SetPlayerEffect(EffectType effectType, float duration = 5)
    {
        playerEffectsHandler.AddEffect(effectType, duration);
    }

    [Command("player_set_health", "Increases / Decreases Player Health")]
    public void IncreaseHealth(float value)
    {
        playerAttributes.IncreaseHealth(value);
    }

    [Command("player_set_energy", "Increases / Decreases Player Energy")]
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

    [Command("player_set_flying", "Sets Player Flight Bool")]
    public void SetPlayerFlying(bool flying, float flyingSpeed = 10)
    {
        playerController.SetFlying(flying, flyingSpeed);
    }   

    [Command("player_set_flying_speed", "Sets Player Flight Speed")]
    public void SetPlayerFlyingSpeed(float flyingSpeed = 10)
    {
        playerController.SetFlySpeed(flyingSpeed);
    }     

    [Command("player_set_gravity", "Sets Player Gravity")]
    public void SetPlayerGravity(float value)
    {
        playerController.SetGravity(value);
    }

    [Command("player_mesh_toggle", "Toggles Player Mesh")]
    public void PlayerToggleMesh(bool toggle)
    {
        playerArmature.SetActive(toggle);
    }




    //MIC COMMANDS
    //////////////
    [Command("spawn_entity", "Spanws an Itemn Entity at a Position")]
    public void SpawnItemAtPosition([ItemName]string itemName, Vector3 position, int count = 1)
    {
        ItemSO itemToSpawn = itemDatabase.GetMatchingItem(itemName);
        itemSpawner.SpawnItem(itemToSpawn, position, count);
    }

    [Command("summon", "Summons An Entity")]    
    public void SpawnEntityAtPosition([EntityName]string entityName, Vector3 position)
    {
        GameObject entityToSpawn = entityDatabase.GetMatchingEntity(entityName);
        entitySpawner.SpawnEntity(entityToSpawn, position);
    }

    [Command("spawn_prefab", "Spanws a Prefab at a Position")]
    public void SpawnPrefabAtPosition([PrefabName]string prefabName, Vector3 position, int count = 1)
    {
        GameObject objectToSpawn = entityDatabase.GetMatchingPrefab(prefabName);
        entitySpawner.SpawnEntity(objectToSpawn, position, count);
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
