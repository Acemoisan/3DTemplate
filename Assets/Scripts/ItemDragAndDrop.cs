/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ItemDragAndDrop : MonoBehaviour
{    
    [Header("Dependencies")]
    [SerializeField] PlayerInventory _playerInventoryReference;
    //[SerializeField] ItemSpawnerSO _itemSpawnerSO;


    //[SerializeField] Transform dropPos;
    Sprite dragAndDropIcon;
    [SerializeField] ItemSlot itemSlot;



    private void Start()
    {
        itemSlot = new ItemSlot();
    }

    public void SetDragAndDropIcon(Sprite icon)
    {
        dragAndDropIcon = icon;
    }

 

    internal void OnClick(ItemSlot itemSlot)
    {
        //if the placeholder is empty - Pickup item
        if (this.itemSlot.item == null)            //PICKING UP ITEM

        {
            PickUpItem(itemSlot);

        }
        //if the placeholder is full - Place Item
        else                                      //PLACING ITEM

        {
            PlaceItem(itemSlot);
        }
    }


    void PlaceItem(ItemSlot itemSlot)
    {
        //STACKING ITEMS
        if (this.itemSlot.item == itemSlot.item && this.itemSlot.count >= 1)
        {
            if (itemSlot.count + this.itemSlot.count < _playerInventoryReference.GetMaxItemStackNumber())
            {
                itemSlot.count += this.itemSlot.count;
                this.itemSlot.Clear();
            }
            else
            {
                //if greater than 99. only add enough to equal 99
                int amountToAdd;
                amountToAdd = _playerInventoryReference.GetMaxItemStackNumber() - itemSlot.count;
                itemSlot.count += amountToAdd;
                this.itemSlot.count -= amountToAdd;
            }
        }
        else
        {
            //Create a new ItemSO from the data abvove. From ***InventorySlot[4]
            ItemSO item = itemSlot.item;
            int count = itemSlot.count;
            //Give ***InventorySlot[4] the data from this placeholder. 
            itemSlot.Copy(this.itemSlot);
            this.itemSlot.Set(item, count);
        }
    }


    void PickUpItem(ItemSlot itemSlot)
    {
        //Set this new placeholder ItemSlot with the data from the selected item ***InventorySlot[4]       
        this.itemSlot.Copy(itemSlot);

        //And then clear ***InventorySlot[4]
        itemSlot.Clear();
    }


    internal void OnClickGarbage(ItemSlot itemSlot)
    {
        if (this.itemSlot.item == null)
        {
            this.itemSlot.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            //AN ITEM IS IN GARBAGE ALREADY
            if (itemSlot.item != null)
            {

                itemSlot.Clear();

            }
            ItemSO item = itemSlot.item;
            int count = itemSlot.count;
            itemSlot.Copy(this.itemSlot);
            this.itemSlot.Set(item, count);
        }
        //UpdateIcon();
    }

    public void DropItem() //called on button click
    {
        if (this.itemSlot.item == null)
        {
            return;
        }
        else
        {
            //_itemSpawnerSO.SpawnItem(this.itemSlot.item, dropPos.position, this.itemSlot.count);            
            this.itemSlot.Clear();
            SetDragAndDropIcon(null);
        }        
    }


    void SplitItemCountInHalf(ItemSlot itemSlot) //This is used to split the item count in half when the player presses the binded button

    {
        if (this.itemSlot.item == null)            //PICKING UP ITEM 
        {
            this.itemSlot.item = itemSlot.item; //setting placeholder item as item picked up
            this.itemSlot.count = itemSlot.count / 2; //setting placeholdercount as half

            itemSlot.count = itemSlot.count / 2; //setting picked up item count as half
        }
    }


    public ItemSlot GetItemSlot()
    {
        return itemSlot;
    }

    public Sprite GetDragAndDropIcon()
    {
        return dragAndDropIcon;
    }
}
