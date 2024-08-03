using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Database", menuName = "Scriptable Objects/Storage/Database/Entity Database")]
public class EntityDatabase : ScriptableObject
{

    public List<GameObject> entities;
    public List<GameObject> prefabs;

    public GameObject GetMatchingEntity(string entityName)
    {
        // Replace underscores with spaces
        string formattedName = entityName.Replace("_", " ");

        foreach (GameObject entity in entities)
        {
            if (entity.name == formattedName)
            {
                // Item found
                return entity;
            }
        }

        // Item not found
        Debug.LogError("<color=#ff0000>" + formattedName + "</color> Not found in Enemy Database");
        return null;
    }

    public GameObject GetMatchingPrefab(string prefabName)
    {
        // Replace underscores with spaces
        string formattedName = prefabName.Replace("_", " ");

        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == formattedName)
            {
                // Item found
                return prefab;
            }
        }

        // Item not found
        Debug.LogError("<color=#ff0000>" + formattedName + "</color> Not found in Enemy Database");
        return null;
    }

    public string GetAllEntityNames()
    {
        string names = "";
        for (int i = 0; i < entities.Count; i++)
        {
            names += entities[i].name + ",";
        }
        return names;
    }

}
