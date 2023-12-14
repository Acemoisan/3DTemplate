// /*
//  *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
//  *  Unauthorized copying of this file, via any medium is strictly prohibited
//  *  Proprietary and confidential
//  */

// using UnityEngine;

// [CreateAssetMenu(fileName = "NewItemSpawner", menuName = "Scriptable Objects/Item Spawner")]
// public class ItemSpawnerSO : ScriptableObject
// {
//     [Header("Dependencies")]
//     [SerializeField] GameObject itemEntity;
//     [SerializeField] InventorySO _playersInventory;

//     //private
//     EntityTrigger itemEntityTrigger;



//     [Header("Items")]
//     public ItemSO shovel;
//     public ItemSO rake;
//     public ItemSO hoe;
//     public ItemSO seeds;




//     public void SpawnItem(ItemSO item, Vector3 itemLocation)
//     {
//         itemEntityTrigger = itemEntity.GetComponentInChildren<EntityTrigger>();
//         itemEntityTrigger.itemToGive = item;
//         itemEntityTrigger.itemIcon.sprite = item.icon;

//         Instantiate(itemEntity, itemLocation, Quaternion.identity);

//         //ADDFORCE
// /*        GameObject entity = Instantiate(itemEntity, itemLocation.position, Quaternion.identity) as GameObject;
//         entity.GetComponent<Rigidbody2D>().AddForce(entity.transform.up * 30, ForceMode2D.Impulse);*/
//     }
// }
