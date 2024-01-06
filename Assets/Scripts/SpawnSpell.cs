using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SpawnSpell : MonoBehaviour//: ToolHit
{
    [SerializeField] ItemSpawnerSO _itemSpawner;
    [SerializeField] TimeManagerSO TimeManagerSO;
    [SerializeField] List<GameObject> spellsToSpawn;
    [SerializeField] Transform _spawnLocation;
    [Range(0, 100)] [SerializeField] int chanceOfDropOutOf100;
    [SerializeField] int dropCountMinimum = 1;
    [SerializeField] int dropcountMaximum = 1;
    [SerializeField] float spreadOfObjects;


    Vector3 spawnPos;
    [SerializeField] GameObject spellToDrop;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        GetCorrectSpellToDrop();
    }


    public void SpawnAnObject()
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

            //GameObject randomitem = spellsToSpawn[Random.Range(0, spellsToSpawn.Count)];
            //int randomChance = Random.Range(1, 101);
            _itemSpawner.SpawnItem(spellToDrop, spawnPos);
        }
    }

    public void GetCorrectSpellToDrop()
    {
        switch(TimeManagerSO.hour)
        {
            case 12:
                spellToDrop = spellsToSpawn[0];
                break;
            case 13:
                spellToDrop = spellsToSpawn[Random.Range(0, 2)];
                break;
            case 14:
                spellToDrop = spellsToSpawn[Random.Range(0, 3)];
                break;
            case 15:
                spellToDrop = spellsToSpawn[Random.Range(0, 4)];
                break;
            case 16:
                spellToDrop = spellsToSpawn[Random.Range(0, 5)];
                break;
            case 17:
                spellToDrop = spellsToSpawn[Random.Range(0, 6)];
                break;
            case 18:
                spellToDrop = spellsToSpawn[Random.Range(0, spellsToSpawn.Count)];
                break;
            default:
                spellToDrop = spellsToSpawn[Random.Range(0, spellsToSpawn.Count)];
                break;
        }
    }
}
