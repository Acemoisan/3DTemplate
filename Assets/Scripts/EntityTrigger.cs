using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrigger : Interactable
{
    //[HideInInspector]
    [SerializeField] GameObject entity;
    [SerializeField] ItemSO itemToGive;
    [SerializeField] int amountToGive = 0;

    //[HideInInspector]
    public SpriteRenderer itemIcon;
    bool allowedToGive = false;
    //[SerializeField] ItemEggSO egg;
    

    //void Start()
    //{
        //Invoke("SetAllowedToGive", 1f);
    //}


    public void GiveItemToPLayer()
    {
        if (GetPlayerInventory().CanFitItemInIventory(itemToGive, amountToGive))
        {

            GetPlayerInventory().AddItem(itemToGive, amountToGive);

            Destroy(entity);        
        }
    }


    public void GiveCurrencyToPlayer(int amount)
    {
        GetPlayerInventory().AddCurrency(amount);
        Destroy(transform.parent.parent.gameObject);    
    }


    PlayerInventory GetPlayerInventory()
    {
        if(GetPlayer() == null) { Debug.LogError("Interactee not found"); return null; }
        if(GetPlayer().GetComponentInChildren<PlayerInventory>() == null) { Debug.LogError("PlayerInventory not found on " + GetPlayer()); return null; }
        
        return GetPlayer().GetComponentInChildren<PlayerInventory>();   
    }

    public void SetupEntity(ItemSO item, int count)
    {
        itemToGive = item;
        amountToGive = count;

        //this.egg = item as ItemEggSO;
        itemIcon.sprite = item.GetIcon();
    }


    void SetAllowedToGive()
    {
        allowedToGive = true;
    }

    public ItemSO GetItemToGive()
    {
        return itemToGive;
    }

    public void DestroyEntity()
    {
        if(entity == null) { Debug.LogError("Entity not found on " + this); }
        Destroy(entity);
    }
}