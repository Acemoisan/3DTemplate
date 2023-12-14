/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSpawner", menuName = "Scriptable Objects/Utility/Item Spawner")]
public class ItemSpawnerSO : ScriptableObject
{
    [Header("Dependencies")]
    [SerializeField] GameObject itemEntity;
    //[SerializeField] InventorySO _playersInventory;

    //private
    EntityTrigger itemEntityTrigger;


    // public void SpawnItemInPlayerInventory(InventorySO playerInventory)
    // {
    //     playerInventory.AddItem(_itemsToSpawn[Random.Range(0, _itemsToSpawn.Count)]);
    // }


    public void SpawnItem(ItemSO item, Vector3 itemLocation, int count = 1, Transform parent = null)
    {
        itemEntityTrigger = itemEntity.GetComponentInChildren<EntityTrigger>();

        //ItemEggSO animalEggRef = item as ItemEggSO;

        itemEntityTrigger.SetupEntity(item, count);
        Instantiate(itemEntity, itemLocation, Quaternion.identity, parent);
        //AddForce();
    }

    public void SpawnItemAndGetGameObject(ItemSO item, Vector3 itemLocation, out GameObject spawnedItem, int count = 1, Transform parent = null)
    {
        itemEntityTrigger = itemEntity.GetComponentInChildren<EntityTrigger>();
        itemEntityTrigger.SetupEntity(item, count);

        spawnedItem = Instantiate(itemEntity, itemLocation, Quaternion.identity, parent);

        //AddForce();
    }

    public void SpawnItem(GameObject item, Vector3 itemLocation)
    {
        Instantiate(item, itemLocation, Quaternion.identity);
    }

    void AddForce(GameObject entity)
    {
        //if(entity.GetComponent<Rigidbody2D>() == null) { Debug.LogError("Rigidbody2D not found on " + entity.name); return; }
        // entity.GetComponent<Rigidbody2D>().AddForce(entity.transform.up * 30, ForceMode2D.Impulse);
    }
}
