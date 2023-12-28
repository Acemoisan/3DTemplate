using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [Header("------ DEPENDENCIES ------")]
    [SerializeField] InventoryUI1 inventoryUI;
    //public InventorySO _inventorySO;
    //[SerializeField] PopUpIndicatorUI pickedUpItemUIReference;
    //[SerializeField] PlayerQuestManager playerQuestManager;



    [Header("------ CONFIGURABLES ------")]
    [SerializeField] int maxItemStackNumber;
    [SerializeField] Transform primaryHandPosition;

    

    [Header("------ CURRENCIES ------")]
    [SerializeField] int playerCurrency;



    [Header("------ ACTIVE ITEMS ------")] 
        [SerializeField] bool charmActive = false;

    [SerializeField] ItemSO _activeItem; //this is set on the HUDUI
    [SerializeField] ItemSO _activePotion; //this is set on the PotionUI
    ItemSlot activatedItemSlot;



    // [Header("------ CHECKS ------")]
    // public bool inventoryFull;



    [Header("------ ITEM LISTS ------")]
    [SerializeField] int availableInventorySlots;
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> hotBarSlots = new List<ItemSlot>();
    public List<ItemSlot> miscSlots = new List<ItemSlot>();






    #region //////////////// ADD / REMOVE ITEM ////////////////////
    ItemSlot newItemSlot;


    //IMPORTANT HOW TO ============================================================================================================== IMPORTANT HOW TO


    //When I add an item. I am checking the list of "ItemSlots" (ItemSO). 
    //I am finding an empty itemslot using: ItemSlot itemSlot = inventorySlots.Find(x => x.item == null);
    //If the ItemSlot is empty. I am replacing it with the "item" that i am passing through the function. 
    //If the item is IsStackable(). Before finding an empty slot. I am trying to find the item itself. and increasing the count of it
    //If the item is not found. I am replacing an empty slot with the new item.

    public void CharmActive(bool trigger) //activated when charm button clicked from charm ui
    {
        charmActive = trigger;
    }


    public void SetActiveItem(ItemSO activatedItem)
    {
        _activeItem = activatedItem;

        foreach (Transform child in primaryHandPosition)
        {
            child.gameObject.SetActive(false);
        }

        //check all children of hand position, if item"HandPrefab" is not a child instantiate object in hand position. if it is a child. set it to active
        if(_activeItem != null)
        {
            if(_activeItem.GetHandPrefab() != null)
            {
                Debug.Log("Hand Prefab: " + _activeItem.GetHandPrefab().name);
                foreach (Transform child in primaryHandPosition)
                {
                    if(child.gameObject.name == _activeItem.GetHandPrefab().name)
                    {
                        child.gameObject.SetActive(true);
                        return;
                    }
                }

                GameObject handItem = Instantiate(_activeItem.GetHandPrefab(), primaryHandPosition);
                handItem.name = _activeItem.GetHandPrefab().name;
                handItem.SetActive(true);
            }
        }
    }


    public void AddItem(ItemSO item)
    {
        AddItem(item, 1);
    }

    public void AddItem(ItemSO pickupItem, int count = 1)
    {
        if (pickupItem.IsStackable())
        {
            FindMatchingItemSlot(pickupItem, count);


            AddItemToSlot(pickupItem, count);
            Debug.Log("Added " + pickupItem.GetItemName() + " to inventory");

        }
        //When adding a new ItemSO. if item can **NOT** be stacked. Simply add it to inventory by replacing an empty slot with the item passed. 
        else
        {
            //Finding an empty slot with no item in it. 

            for(int i = 0; i < count; i++)
            {
                ItemSlot hotBarItemSlot = hotBarSlots.Find(x => x.item == null);

                //if hotbar is not full. add to it
                if (hotBarItemSlot != null)
                {
                    hotBarItemSlot.item = pickupItem;
                }
                //if hotbar is full 
                else
                {
                    ItemSlot inventoryItemSlot = inventorySlots.Find(x => x.item == null);
                    if (inventoryItemSlot != null)
                    {
                        inventoryItemSlot.item = pickupItem;
                    }
                }
            }
        }

        //pickedUpItemUIReference.ShowPopup(pickupItem.GetIcon(), pickupItem.GetItemName(), count);
        inventoryUI.UpdateHotBarItemCount();
        //playerQuestManager.ItemCollectedForGoal(pickupItem, count);
    }

    void FindMatchingItemSlot(ItemSO pickupItem, int count)
    {
        ItemSlot newHotBarSlot = hotBarSlots.Find(x => x.item == pickupItem && x.count < GetMaxItemStackNumber()); //FIND MATHCING ITEM HOTBAR SLOT

        //FIND INVENTORTY SLOTS AS WELL
        if(newHotBarSlot != null)
        {
            newItemSlot = newHotBarSlot;
        }
        else {
            ItemSlot emptySlot = hotBarSlots.Find(x => x.item == null); //FIND EMPTY HOTBAR SLOT

            if(emptySlot != null)
            {
                newItemSlot = emptySlot;
            }
            else
            {
                ItemSlot newInventorySlot = inventorySlots.Find(x => x.item == pickupItem && x.count < GetMaxItemStackNumber()); //FIND MATHCING ITEM INVENTORY SLOT
                if (newInventorySlot != null)
                {
                    newItemSlot = newInventorySlot;
                }
                else
                {
                    for(int availableSlots = 0; availableSlots < GetAvailableInventoryCount(); availableSlots++)
                    {
                        if(inventorySlots[availableSlots].item == null)
                        {
                            newItemSlot = inventorySlots[availableSlots];
                            return;
                        }              
                    }

                    newItemSlot = null;
                }
            }
        }    
    }

    void AddItemToSlot(ItemSO pickupItem, int count)
    {
        //IF ITEM SLOT + PICKUP IS GREATER THAN MAX
        if (newItemSlot.count + count > GetMaxItemStackNumber())
        {
            newItemSlot = hotBarSlots.Find(x => x.item == null); //FIND EMPTY HOTBAR SLOT
            if (newItemSlot != null)
            {
                newItemSlot.item = pickupItem;
                newItemSlot.count = count;
            }
            else
            {
                ItemSlot newInventorySlot = inventorySlots.Find(x => x.item == pickupItem && x.count < GetMaxItemStackNumber()); //FIND MATHCING ITEM INVENTORY SLOT
                if(newInventorySlot != null)
                {
                    newItemSlot = newInventorySlot;
                    newItemSlot.count += count;
                }
                else {
                    newItemSlot = inventorySlots.Find(x => x.item == null); //FIND EMPTY INVENTORY SLOT
                    newItemSlot.item = pickupItem;
                    newItemSlot.count = count;
                }
            }
        }
        //IF ITEM SLOT + COUNT IS LESS THAN MAX. SIMPLY ADD COUNT + ITEM
        else
        {
            newItemSlot.item = pickupItem;
            newItemSlot.count += count;
        }    
    }    

    public void RemoveItemFromHotbar(ItemSO itemToRemove, int count = 1)
    {

        if (itemToRemove.IsStackable())
        {

            ItemSlot itemSlot = hotBarSlots.Find(x => x.item == itemToRemove);

            if (itemSlot == null) { return; }

            itemSlot.count -= count;
            if (itemSlot.count <= 0)
            {
                Invoke("ClearLastItem", .1f); //must delay because when a last item is destroyed the action afterwards is not called
                itemSlot.Clear();
            }

        }
        else
        {
            ItemSlot itemSlot = hotBarSlots.Find(x => x.item == itemToRemove);
            if (itemSlot == null) { return; }
            itemSlot.Clear(); 
            Invoke("ClearLastItem", .1f); //must delay because when a last item is destroyed the action afterwards is not called
        }

        inventoryUI.UpdateHotBarItemCount();
    }

    public void RemoveItemFromMainInventory(ItemSO itemToRemove, int count = 1)
    {
        int tempCount = 0;

        if (itemToRemove.IsStackable())
        {
            ItemSlot itemSlot = inventorySlots.Find(x => x.item == itemToRemove);

            if (itemSlot == null) { return; }

            tempCount = count - itemSlot.count;
            itemSlot.count -= count;

            if (itemSlot.count <= 0)
            {
                Invoke("ClearLastItem", .1f); //must delay because when a last item is destroyed the action afterwards is not called (planting seed)
                itemSlot.Clear();
            }

            if(tempCount > 0)
            {
                RemoveItemFromMainInventory(itemToRemove, tempCount);
            }

        }
        else
        {
            ItemSlot itemSlot = inventorySlots.Find(x => x.item == itemToRemove);
            if (itemSlot == null) { return; }
            itemSlot.Clear();           
        }
    }

    public void ClearLastItem()
    {
        _activeItem = null;
    }

    public void ClearInventory()
    {
        foreach(ItemSlot slot in inventorySlots)
        {
            slot.Clear();
        }

        foreach(ItemSlot slot in hotBarSlots)
        {
            slot.Clear();
        }

        foreach(ItemSlot slot in miscSlots)
        {
            slot.Clear();
        }

        inventoryUI.UpdateHotBarItemCount();
    }
    #endregion 





    #region //////////////// CHECKS /// GETS ////////////////////

    public bool CanFitItemInIventory(ItemSO item, int count = 1)
    {
        if (item.IsStackable())
        {
            for(int hotbarSlot = 0; hotbarSlot < hotBarSlots.Count; hotbarSlot++)
            {
                if(hotBarSlots[hotbarSlot].item == item && hotBarSlots[hotbarSlot].count < GetMaxItemStackNumber())
                {
                    return true;
                }
                else if(hotBarSlots[hotbarSlot].item == null)
                {
                    return true;
                }
            }

            for(int availableSlots = 0; availableSlots < GetAvailableInventoryCount(); availableSlots++)
            {
                if(inventorySlots[availableSlots].item == item && inventorySlots[availableSlots].count < GetMaxItemStackNumber())
                {
                    return true;
                }

                if(inventorySlots[availableSlots].item == null)
                {
                    return true;
                }              
            }

            return false;
        }
        else
        {
            for(int hotbarSLots = 0; hotbarSLots < hotBarSlots.Count; hotbarSLots++)
            {
                if(hotBarSlots[hotbarSLots].item == null)
                {
                    return true;
                }
            }

            for(int availableSlots = 0; availableSlots < GetAvailableInventoryCount(); availableSlots++)
            {
                if(inventorySlots[availableSlots].item == null)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public int GetCountOfItemInInventory(ItemSO item)
    {
        int itemCount = 0;

        foreach (ItemSlot itemSlot in inventorySlots)
        {
            if(itemSlot.item == item)
            {
                itemCount += itemSlot.count;
            }
        }

        foreach (ItemSlot itemSlot in hotBarSlots)
        {
            if(itemSlot.item == item)
            {
                itemCount += itemSlot.count;
            }
        }

        return itemCount;
        
    }     

    public ItemSO GetActiveItem()
    {
        ItemSO item = _activeItem;
        return item != null ? item : null;
    }

    public ItemSlot GetActiveItemSlot()
    {
        return activatedItemSlot != null ? activatedItemSlot : null;
    }


    public int GetAvailableInventoryCount()
    {
        return availableInventorySlots;
    }

    public int GetMaxItemStackNumber()
    {
        return maxItemStackNumber;
    }

    public int GetPlayerCurrencyValue()
    {
        return playerCurrency;
    }
    #endregion





    #region //////////////// ADD / REMOVE CURRENCY ////////////////////
    //Add certain currency Types

    public void AddCurrency(int amount)
    {
        this.playerCurrency += Mathf.Abs(amount);
        if (playerCurrency < 0) { playerCurrency = 0; }
        //updateUIEvent.Raise();
    }

    public void RemoveCurrency(int amount)
    {
        this.playerCurrency -= Mathf.Abs(amount);
        //updateUIEvent.Raise();
    }

    public void RemoveCurrencyOnDeath() //called by on death function on player attributes
    {
        int roundedCurrency = Mathf.RoundToInt(playerCurrency / 10);
        this.playerCurrency -= roundedCurrency;
        //updateUIEvent.Raise();
    }

    public void SetCurrency(int amount)
    {
        this.playerCurrency = Mathf.Abs(amount);
        //updateUIEvent.Raise();
    }
    #endregion


    public void IncreaseInventorySize()
    {
        availableInventorySlots++;
    }
}
