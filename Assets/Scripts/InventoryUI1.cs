/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class InventoryUI1 : MonoBehaviour
{
    [Header("Player Dependencies")]
    //[SerializeField] PlayerAttributes _playerAttributes;
    [SerializeField] PlayerInventory _playerInventory;
    //[SerializeField] PlayerMagic _playerMagic;
    [SerializeField] TextMeshProUGUI _playersNameText;
    [SerializeField] Image _playersIcon;



    [Header("Pet Dependencies")]
    //public EntityAttributesSO _petAttributesSO;
    [SerializeField] TextMeshProUGUI _petsNameText;
    [SerializeField] Image _petsIcon;



    [Header("General Dependencies")]
    public ToolTip toolTip;



    [Header("General Dependencies / Lists")]
    public List<InventoryButton> mainInventoryButtons;
    public List<InventoryButton> hotBarButtons;
    public List<InventoryButton> miscButtons;
    [SerializeField] List<UnityEngine.GameObject> inventoryPages;
    [SerializeField] List<Image> inventoryTabButtons;



    [Header("General Dependencies / Sprites")]
    //[SerializeField] Image lockIcon;
    [SerializeField] Sprite lockedTexture;
    [SerializeField] Sprite unlockedTexture;
    [SerializeField] Sprite inactiveInventoryTabSprite;
    [SerializeField] Sprite activeInventoryTabSprite;






    //private 
    int tab = 0;
    bool hotBarLocked = false;
    bool inventoryToggled = false;





    public void SetUpInventory() //Called on canvas changer. OnInventoryInvoke
    {
        //PLAYER
        //_playersNameText.text = _playerAttributes.GetPlayerName();
        //_playersIcon.sprite = _playerAttributes.GetPlayerSprite();        


        //BUTTONS
        SetIndexOfAllButtons();
        ShowButtons();


        //TABS
        ChangeInventoryTab(0);
    }





    
    public void UpdateHotBarItemCount()
    {
        //HOTBAR BUTTONS
        for (int i = 0; i < _playerInventory.hotBarSlots.Count && i < hotBarButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            if (_playerInventory.hotBarSlots[i].item == null)
            {
                hotBarButtons[i].CleanButtons();
            }
            else
            {
                hotBarButtons[i].SetButtonSprites(_playerInventory.hotBarSlots[i]);
            }
        }
    }
 
 
    #region ///////////// INVENTORY SLOTS ////////////////

    public void ShowButtons()
    {
        ShowAllButtons();
        TurnOffUnavailableButtons();
    }

    public void ShowAllButtons()
    {
        //MAIN INVENTORY
        for (int i = 0; i < _playerInventory.inventorySlots.Count && i < mainInventoryButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            if (_playerInventory.inventorySlots[i].item == null)
            {
                mainInventoryButtons[i].CleanButtons();
            }
            else
            {
                mainInventoryButtons[i].SetButtonSprites(_playerInventory.inventorySlots[i]);
            }
        }

        //HOTBAR BUTTONS
        for (int i = 0; i < _playerInventory.hotBarSlots.Count && i < hotBarButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            if (_playerInventory.hotBarSlots[i].item == null)
            {
                hotBarButtons[i].CleanButtons();
            }
            else
            {
                hotBarButtons[i].SetButtonSprites(_playerInventory.hotBarSlots[i]);
            }
        }

        //MISC BUTTONS
        for (int i = 0; i < _playerInventory.miscSlots.Count && i < miscButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            if (_playerInventory.miscSlots[i].item == null)
            {
                miscButtons[i].CleanButtons();
            }
            else
            {
                miscButtons[i].SetButtonSprites(_playerInventory.miscSlots[i]);
            }
        }
    }   


    // public void ShowAllSpellsOnButtons()
    // {
    //     //HOTBAR BUTTONS
    //     for (int i = 0; i < 10; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
    //     {
    //         hotBarButtons[i].CleanButtons();
    //     }

    //     for (int i = 0; i < _playerMagic.spells.Count; i++)
    //     {
    //         if (_playerMagic.spells[i] == null) { continue; }
    //         hotBarButtons[i].SetButtonSprites(_playerMagic.spells[i]);
    //     }
    // }   

    void TurnOffUnavailableButtons()
    {
        for (int i = 0; i < mainInventoryButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            mainInventoryButtons[i].gameObject.GetComponent<Button>().enabled = false;
            mainInventoryButtons[i].gameObject.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
        }

        for (int i = 0; i < _playerInventory.GetAvailableInventoryCount(); i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            mainInventoryButtons[i].gameObject.GetComponent<Button>().enabled = true;
            mainInventoryButtons[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    } 


    //Setting number index to every single button inside of Main inventory Slot Panel
    public void SetIndexOfAllButtons()
    {

        //MAIN INVENTORY
        for (int i = 0; i < _playerInventory.inventorySlots.Count && i < mainInventoryButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            mainInventoryButtons[i].SetIndex(i);
        }

        //HOTBAR INVENTORY
        for (int i = 0; i < _playerInventory.hotBarSlots.Count && i < hotBarButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            hotBarButtons[i].SetIndex(i);
        }

        //EQUIPABLE INVENTORY
        for (int i = 0; i < _playerInventory.miscSlots.Count && i < miscButtons.Count; i++)// && i < _playerInventory.availableInventorySlots.Count; i++)
        {
            miscButtons[i].SetIndex(i);
        }

    }


    public void LockandUnlockHotbar()
    {
        //default is unlocked
        if (!hotBarLocked)
        {
            for (int i = 0; i < hotBarButtons.Count; i++)
            {
                hotBarButtons[i].SetButtonTag(ButtonTag.Locked);
            }
            hotBarLocked = true;
            //lockIcon.sprite = lockedTexture;
        }
        else
        {
            for (int i = 0; i < hotBarButtons.Count; i++)
            {
                hotBarButtons[i].SetButtonTag(ButtonTag.Hotbar);
            }
            hotBarLocked = false;
            //lockIcon.sprite = unlockedTexture;

        }

    }

    #endregion





    #region ///////////// INVENTORY TABS ////////////////
     
    public void ChangeInventoryTab(int tabNumber) //used for button onclick (KEYBOARD)
    {
        ChangeInventoryPage(tabNumber);

        ChangeTabSprite(tabNumber);

        tab = tabNumber;
    }

    public void NextTab(InputAction.CallbackContext context) //used for controller bumpers
    {
        if (!context.performed) return;

        if (tab < 5)
        {
            tab++;
        }
        else
        {
            tab = 0;
        }

        ChangeInventoryPage(tab);

        ChangeTabSprite(tab);

    }

    public void PreviousTab(InputAction.CallbackContext context) //used for controller bumpers
    {

        if (!context.performed) return;

        if (tab > 0)
        {
            tab--;
        }
        else
        {
            tab = 5;
        }

        ChangeInventoryPage(tab);

        ChangeTabSprite(tab);
    }

    void ChangeInventoryPage(int tab)
    {
        foreach (UnityEngine.GameObject tabIndex in inventoryPages)
        {
            tabIndex.gameObject.SetActive(false);
        }
        inventoryPages[tab].SetActive(true);
        if(inventoryPages[tab].GetComponent<InventoryTab>() == null) {Debug.Log("No Inventory Tab Component on " + inventoryPages[tab].name); return; }
        inventoryPages[tab].GetComponent<InventoryTab>().OnTabUpdate();
    }

    void ChangeTabSprite(int tab)
    {
        //changing tab icon
        foreach (Image tabButton in inventoryTabButtons)
        {
            tabButton.sprite = inactiveInventoryTabSprite;
        }
        inventoryTabButtons[tab].sprite = activeInventoryTabSprite;
    }

    #endregion
}
