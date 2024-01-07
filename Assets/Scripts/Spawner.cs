using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("<color=#19bfbf>XXXXXXXXXXXXXXXXX</color>")]
    [Header("<color=#ffffff>XXXX SPAWNER XXXX</color>")]
    [SerializeField] TimeManagerSO TimeManagerSO;
    [SerializeField] protected List<UnityEngine.GameObject> prefabsToSpawn;
    [SerializeField] protected UnityEngine.GameObject itemParent;
    [SerializeField] protected int minimumEntitiesToSpawn;
    [SerializeField] protected int maximumEntitiesToSpawn;
    //[SerializeField] protected List<Collider2D> spawnAreas;
    [SerializeField] List<Transform> spawnAreas;
    


    [Header("   Spawn Over Time Values")]
    [SerializeField] protected bool spawnOnStart;
    [SerializeField] protected bool spawnEnemiesAtOnce;
    [SerializeField] protected bool spawnEnemiesOverTime;
    [SerializeField] protected float spawnRate;



    protected List<UnityEngine.GameObject> entitiesSpawned = new List<UnityEngine.GameObject>();
    Collider2D selectedSpawnArea;

    [SerializeField] UnityEngine.GameObject player;



    protected Transform GetRandomPosition()
    {
        Transform closest = null;
        Transform secondClosest = null;

        float closestDistance = Mathf.Infinity;
        float secondClosestDistance = Mathf.Infinity;

        // Iterate through all spawn points to find the closest and second closest points
        foreach (Transform spawnPoint in spawnAreas)
        {
            float distance = Vector3.Distance(player.transform.position, spawnPoint.position);

            // Check if this point is the closest so far
            if (distance < closestDistance)
            {
                // Shift the current closest to second closest
                secondClosest = closest;
                secondClosestDistance = closestDistance;

                closest = spawnPoint;
                closestDistance = distance;
            }
            else if (distance < secondClosestDistance)
            {
                // This point isn't the closest but is the second closest
                secondClosest = spawnPoint;
                secondClosestDistance = distance;
            }
        }

        Transform selectedSpawnPoint;

        // Choose which spawn point you want to use: closest or second closest
        if(Random.Range(0, 2) == 0)
        {
            selectedSpawnPoint = closest; // or secondClosest for the next closest
        }
        else
        {
            selectedSpawnPoint = secondClosest; // or secondClosest for the next closest
        }
        if(selectedSpawnPoint != null)
        {
            return selectedSpawnPoint;
        }

        return spawnAreas[Random.Range(0, spawnAreas.Count)]; //do this if nothing else is triggered

    }

    // protected Collider2D GetRandomSpawnArea()
    // {
    //     Collider2D randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
    //     selectedSpawnArea = randomArea;
    //     return GetSelectedSpawnArea();
    // }

    public Collider2D GetSelectedSpawnArea()
    {
        return selectedSpawnArea;
    }

    protected void Spawn(out UnityEngine.GameObject spawnedEntity)
    {
        UnityEngine.GameObject randomEntity = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];
        UnityEngine.GameObject entity = Instantiate(randomEntity);
        entity.name = randomEntity.name;
        entity.transform.position = GetRandomPosition().position;
        entity.transform.parent = itemParent.transform;

        spawnedEntity = entity;
    }

    protected void SpawnBasedOnTime(out UnityEngine.GameObject spawnedEntity)
    {
        UnityEngine.GameObject randomEntity = GetRandomEntityBasedOnTime();
        UnityEngine.GameObject entity = Instantiate(randomEntity);
        entity.name = randomEntity.name;
        entity.transform.position = GetRandomPosition().position;
        entity.transform.parent = itemParent.transform;

        spawnedEntity = entity;
    }

    UnityEngine.GameObject GetRandomEntityBasedOnTime()
    {
        switch(TimeManagerSO.hour)
        {
            case 12:
                return prefabsToSpawn[0];
            case 13:
                return prefabsToSpawn[Random.Range(0, 2)];
            case 14:
                return prefabsToSpawn[Random.Range(0, 3)];
            case 15:
                return prefabsToSpawn[Random.Range(0, 4)];
            case 16:
                return prefabsToSpawn[Random.Range(0, 5)];
            case 17:
                return prefabsToSpawn[Random.Range(0, 6)];
            case 18:
                return prefabsToSpawn[Random.Range(0, 7)];
            default:
                return prefabsToSpawn[Random.Range(0, 7)];
        }    
    }

    float spawnRateMultipler = 0.7f;
    protected float GetSpawnRateBasedOnTime()
    {
        switch(TimeManagerSO.hour)
        {
            case 12:
                return spawnRate;
            case 13:
                return spawnRate * spawnRateMultipler;
            case 14:
                return spawnRate * (spawnRateMultipler - .1f);
            case 15:
                return spawnRate * (spawnRateMultipler - .2f);
            case 16:
                return spawnRate * (spawnRateMultipler - .3f);
            case 17:
                return spawnRate * (spawnRateMultipler - .4f);
            case 18:
                return spawnRate * (spawnRateMultipler - .4f);
            default:
                return spawnRate * (spawnRateMultipler - .4f);
        }    
    }
}
