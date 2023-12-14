/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableItem
{
    public ItemSO item;
    [Range(0, 100)] public int chance;

    public SpawnableItem(ItemSO item, int chance)
    {
        this.item = item;
        this.chance = chance;
    }
}


public class SpawnItem : MonoBehaviour//: ToolHit
{
    [SerializeField] ItemSpawnerSO _itemSpawner;
    [SerializeField] List<SpawnableItem> _spawnableItems;
    [SerializeField] Transform _spawnLocation;
    [Range(0, 100)] [SerializeField] int chanceOfDropOutOf100;
    [SerializeField] int dropCountMinimum = 1;
    [SerializeField] int dropcountMaximum = 1;
    [SerializeField] float spreadOfObjects;


    Vector3 spawnPos;

    public void AddSpawnableItem(ItemSO item, int chance)
    {
        _spawnableItems.Add(new SpawnableItem(item, chance));
    }


    public void SpawnAnItem()
    {

        if(Random.Range(1, 101) > chanceOfDropOutOf100) { return; }


        int randomCount = Random.Range(dropCountMinimum, dropcountMaximum);

        //spawn a certain count
        while (randomCount > 0)
        {
            randomCount -= 1;

            if (_spawnLocation != null)
            {
                spawnPos = _spawnLocation.position;
            }
            else 
            {
                spawnPos = this.transform.position;
            }

            spawnPos.x += spreadOfObjects * Random.value - spreadOfObjects / 2;
            spawnPos.y += spreadOfObjects * Random.value - spreadOfObjects / 2;

            SpawnableItem randomitem = _spawnableItems[Random.Range(0, _spawnableItems.Count)];
            int randomChance = Random.Range(1, 101);
            if(randomChance > randomitem.chance) { return; }
            _itemSpawner.SpawnItem(_spawnableItems[Random.Range(0, _spawnableItems.Count)].item, spawnPos);
        }
    }
}
