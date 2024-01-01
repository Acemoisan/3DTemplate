using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributes))]
[RequireComponent(typeof(PlayerInventory))]
public class LoadPlayerData : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] HUDUI hudReference;



    PlayerAttributes _playerAttributes;
    PlayerInventory _playerInventory;


    void Awake()
    {
        if (GetComponent<PlayerAttributes>() == null) { Debug.Log("Player Attributes Not Found"); } else { _playerAttributes = GetComponent<PlayerAttributes>(); }
        if (GetComponent<PlayerInventory>() == null) { Debug.Log("Player Inventory Not Found"); } else { _playerInventory = GetComponent<PlayerInventory>(); }
    }

    void Start()
    {
        _playerAttributes.SetPlayerName(saveSystem.playerLoadingName);
        saveSystem.playerLoadingName = ""; //resetting the name back to null, so that if another player joins with no name. It will not load the previous player's data.

        hudReference.StoreButtonNavigation();
        
        //TriggerCharacterCustomizationMenu();

        saveSystem.LoadPlayerAttributes(_playerAttributes);
        saveSystem.LoadPlayerInventory(_playerInventory, _playerAttributes.GetPlayerName());
        //LoadGlobalData();
    }

    void LoadIndividualData()
    {

        saveSystem.LoadPlayerAttributes(_playerAttributes);
        saveSystem.LoadPlayerInventory(_playerInventory, _playerAttributes.GetPlayerName());
    }

    // void LoadGlobalData()
    // {
    //     foreach(var player in PlayerManager.instance.playerList)
    //     {
    //         if (player.playerName == _playerAttributes.GetPlayerName() && player.isHost)
    //         {
    //             //saveSystem.LoadGlobalData();
    //         }
    //     }    
    // }
}
