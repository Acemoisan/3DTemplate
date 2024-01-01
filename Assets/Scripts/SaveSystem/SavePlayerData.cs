using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributes))]
[RequireComponent(typeof(PlayerInventory))]
public class SavePlayerData : MonoBehaviour
{
    [SerializeField] SaveSystem _saveSystem;
    PlayerAttributes _playerAttributes;
    PlayerInventory _playerInventory;


    void Awake()
    {
        if (GetComponent<PlayerAttributes>() == null) { Debug.Log("Player Attributes Not Found"); } else { _playerAttributes = GetComponent<PlayerAttributes>(); }
        if (GetComponent<PlayerInventory>() == null) { Debug.Log("Player Inventory Not Found"); } else { _playerInventory = GetComponent<PlayerInventory>(); }
    }

    // public void Save()
    // {
    //     Debug.Log("Saving Player Data");
    //     _saveSystem.SaveGame(_playerAttributes, _playerInventory, _playerStatistics);
    //     _saveSystem.SaveSceneData(FindAllPersistantObjects());
    // }
    List<IPersistant> FindAllPersistantObjects()
    {
        List<IPersistant> persistantObjects = new List<IPersistant>();
        foreach (var go in FindObjectsOfType<GameObject>())
        {
            var persistant = go.GetComponent<IPersistant>();
            if (persistant != null)
            {
                persistantObjects.Add(persistant);
            }
        }
        return persistantObjects;
    }
}
