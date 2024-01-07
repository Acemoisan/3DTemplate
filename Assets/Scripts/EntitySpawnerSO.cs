using UnityEngine;



[CreateAssetMenu(fileName = "NewItemSpawner", menuName = "Scriptable Objects/Utility/Entity Spawner")]
public class EntitySpawnerSO : ScriptableObject
{
    // [Header("Dependencies")]
    // [SerializeField] UnityEngine.GameObject itemEntity;
    //[SerializeField] InventorySO _playersInventory;




    public void SpawnEntity(GameObject entity, Vector3 position, int count = 1, Transform parent = null)
    {

        for (int i = 0; i < count; i++)
        {
            Spawn(entity, position, parent);
        }
    }

    public void Spawn(GameObject item, Vector3 itemLocation, Transform parent = null)
    {
        Instantiate(item, itemLocation, Quaternion.identity, parent);
    }
}
