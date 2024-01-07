using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSuggestors : MonoBehaviour
{
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] EntityDatabase entityDatabase;

    public void Initialize()
    {
        ItemNameSuggestor.itemDatabase = itemDatabase;
        EntityNameSuggestor.entityDatabase = entityDatabase;
        PrefabNameSuggestor.entityDatabase = entityDatabase;
    }
}
