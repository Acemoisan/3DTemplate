/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInventory _playersInventory;        
    [SerializeField] List<Button> hotbarInventoryButtons;
    [SerializeField] List<Image> hotbarInventoryButtonImages;
    [SerializeField] UnityEngine.GameObject _activeItemTextObject;
    //[SerializeField] InventoryUI1 _inventoryReference;


    [Header("Text References")]
    [SerializeField] TextMeshProUGUI _activeItemText;
    [SerializeField] TextMeshProUGUI _playersGoldText;


    [Header("Sprite References")]
    [SerializeField] Sprite inactiveSlotSprite;
    [SerializeField] Sprite activeSlotSprite;

    public UnityEvent OnButtonSelected;


    bool canSwitchItem = true;




    //private 
    /////////
    int buttonIndex = 0;
    Navigation navigation0;
    Navigation navigation1;
    Navigation navigation2;
    Navigation navigation3;
    Navigation navigation4;
    Navigation navigation5;
    Navigation navigation6;
    Navigation navigation7;
    Navigation navigation8;
    Navigation navigation9;



    private void Start()
    {
        SetUpHud();
        //DisableNavigation(); //disabling navigation on all HUD buttons
    }

    public void StoreButtonNavigation()
    {
        navigation0 = hotbarInventoryButtons[0].navigation;
        navigation1 = hotbarInventoryButtons[1].navigation;
        navigation2 = hotbarInventoryButtons[2].navigation;
        navigation3 = hotbarInventoryButtons[3].navigation;
        navigation4 = hotbarInventoryButtons[4].navigation;
        navigation5 = hotbarInventoryButtons[5].navigation;
        navigation6 = hotbarInventoryButtons[6].navigation;
        navigation7 = hotbarInventoryButtons[7].navigation;
        navigation8 = hotbarInventoryButtons[8].navigation;
        navigation9 = hotbarInventoryButtons[9].navigation;
    }

    public void SetUpHud() //setup hud is called at start and when the game state is set back to playing
    {
        //_inventoryReference.SetUpInventory(); 

        // if(_playersInventory.IsCharmActive() == false)
        // {
        //     SelectButton(buttonIndex);
        //     SelectPreviouslySelectedButton();
        // }
        // else 
        // {
        //     Debug.Log("Charm is active");
        // }

        DisableButtons();
        DisableNavigation();
       // StartCoroutine(UpdateHUD());
    }

    IEnumerator UpdateHUD()
    {
        while(true)
        {
            UpdateGoldtext();
            yield return new WaitForSeconds(2f);
        }
    }

    public void SelectPreviouslySelectedButton()
    {
        hotbarInventoryButtons[buttonIndex].Select();
    }


    public void SelectHotbarSlotWithKeyboard(float numberKey)
    {
        int number = (int)numberKey;
        SelectButton(number);
    }


    public void SelectButtonWithScrollWheel(float delta)
    {
        if (delta != 0)
        {
            if (delta > 0 )
            {
                buttonIndex += 1;
                if (buttonIndex > 9)
                {
                    buttonIndex = 0;
                }
            }
            else
            {
                buttonIndex -= 1;
                if (buttonIndex < 0)
                {
                    buttonIndex = 9;
                }
            }

            _playersInventory.CharmActive(false);

            SelectButton(buttonIndex);
        }
    }


    public void NextButton() //InputAction.CallbackContext context
    {
        //if (!context.performed) return;

        if (buttonIndex < 9)
        {
            buttonIndex++;
        }
        else
        {
            buttonIndex = 0;
        }
        _playersInventory.CharmActive(false);

        SelectButton(buttonIndex);
    }


    public void PreviousButton()
    {
        if (buttonIndex > 0)
        {
            buttonIndex--;
        }
        else
        {
            buttonIndex = 9;
        }
        _playersInventory.CharmActive(false);

        SelectButton(buttonIndex);
    }


    //TODO: this is the normal code. I am commenting it out to use the spell code
    void SelectButton(int buttonIndex)
    {
        if (GetCanSwitchItem() == false) { return; }
        
        hotbarInventoryButtons[buttonIndex].Select();

        ChangeButtonSprite(buttonIndex);

        SetActivePlayerItem(buttonIndex);     

        OnButtonSelected.Invoke();
    }

    public void HighlightButton(int buttonIndex)
    {
        hotbarInventoryButtons[buttonIndex].Select();
        ChangeButtonSprite(buttonIndex);        
    }


    void ChangeButtonSprite(int buttonIndex)
    {
        foreach (Image hotBarSlot in hotbarInventoryButtonImages)
        {
            hotBarSlot.sprite = inactiveSlotSprite;
        }
        hotbarInventoryButtonImages[buttonIndex].sprite = activeSlotSprite;
    }



    private Coroutine activeItemCoroutine;

    void SetActivePlayerItem(int buttonIndex)
    {
        if (_playersInventory.hotBarSlots[buttonIndex].item != null)
        {
            ItemSO item = _playersInventory.hotBarSlots[buttonIndex].item;
            _playersInventory.SetActiveItem(item);
        }
        else
        {
            _playersInventory.SetActiveItem(null);
            _activeItemTextObject.SetActive(false);
            return;
        }

        if (activeItemCoroutine != null)
        {
            StopCoroutine(activeItemCoroutine);
        }
        activeItemCoroutine = StartCoroutine(ActiveItemText(_playersInventory.hotBarSlots[buttonIndex].item.GetItemName()));
    }

    IEnumerator ActiveItemText(string itemName)
    {
        _activeItemTextObject.SetActive(true);
        _activeItemText.text = itemName;

        // Wait for 2 seconds to allow for quick switching
        yield return new WaitForSeconds(2f);

        // Check if the item name is still the same before hiding
        if (_activeItemText.text == itemName)
        {
            _activeItemTextObject.SetActive(false);
        }
    }



    public void DisableButtons() //buttons disabled when in playing game state
    {
        foreach (Button but in hotbarInventoryButtons)
        {
            but.enabled = false;
        }
    }


    public void EnableButtons() //buttons enabled when in inventory
    {
        foreach (Button but in hotbarInventoryButtons)
        {
            but.enabled = true;
        }
    }


    public void DisableNavigation()
    {
        foreach (Button but in hotbarInventoryButtons)
        {
            Navigation emptyNav = new Navigation();
            emptyNav.mode = Navigation.Mode.None;
            but.navigation = emptyNav;
        }
    }


    public void EnableNavigation() //enabled when in inventory
    {
        hotbarInventoryButtons[0].navigation = navigation0;
        hotbarInventoryButtons[1].navigation = navigation1;
        hotbarInventoryButtons[2].navigation = navigation2;
        hotbarInventoryButtons[3].navigation = navigation3;
        hotbarInventoryButtons[4].navigation = navigation4;
        hotbarInventoryButtons[5].navigation = navigation5;
        hotbarInventoryButtons[6].navigation = navigation6;
        hotbarInventoryButtons[7].navigation = navigation7;
        hotbarInventoryButtons[8].navigation = navigation8;
        hotbarInventoryButtons[9].navigation = navigation9;
    }



    public bool GetCanSwitchItem()
    {
        return canSwitchItem;
    }

    public void UpdateGoldtext()
    {
        int gold = _playersInventory.GetPlayerCurrencyValue();
        _playersGoldText.text = gold.ToString();
    }
}
