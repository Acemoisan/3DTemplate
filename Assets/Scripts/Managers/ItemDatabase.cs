using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Scriptable Objects/Storage/Database/Item Database")]
public class ItemDatabase : ScriptableObject
{

    public List<ItemSO> items;

    public ItemSO GetMatchingItem(string itemName)
    {
        // Replace underscores with spaces
        string formattedItemName = itemName.Replace("_", " ");

        foreach (ItemSO item in items)
        {
            if (item.name == formattedItemName)
            {
                // Item found
                return item;
            }
        }

        // Item not found
        Debug.LogError("<color=#ff0000>" + formattedItemName + "</color> Not found in database");
        return null;
    }

    public string GetItemNames()
    {
        string itemNames = "";
        for (int i = 0; i < items.Count; i++)
        {
            itemNames += items[i].name + ",";
        }
        return itemNames;
    }

}
