using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{

    void Start()
    {
        
        if(spawnOnStart)
        {
            ManuallyStartSpawn();
        }
    }


    public void ManuallyStartSpawn()
    {
        if (spawnEnemiesOverTime)
        {
            StartCoroutine(SpawnOverTime());
        }
        else if (spawnEnemiesAtOnce)
        {
            StartCoroutine(SpawnOnStart());
        }
    }

    IEnumerator SpawnOnStart()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < Random.Range(minimumEntitiesToSpawn, maximumEntitiesToSpawn); i++)
        {
            SpawnEnemy();
        }
    }


    IEnumerator SpawnOverTime()
    {
        List<UnityEngine.GameObject> enemiesToRemove = new List<UnityEngine.GameObject>();

        while (true)
        {
            foreach(UnityEngine.GameObject enemy in entitiesSpawned)
            {
                if(enemy == null)
                {
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach(UnityEngine.GameObject enemy in enemiesToRemove)
            {
                entitiesSpawned.Remove(enemy);
            }

            if (entitiesSpawned.Count < maximumEntitiesToSpawn)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(GetSpawnRateBasedOnTime());
        }
    }


    void SpawnEnemy()
    {
        UnityEngine.GameObject spawnedEntity = null;
        //base.Spawn(out spawnedEntity);
        base.SpawnBasedOnTime(out spawnedEntity);


        if (spawnEnemiesOverTime)
        {
            entitiesSpawned.Add(spawnedEntity);
        }
    }
}
