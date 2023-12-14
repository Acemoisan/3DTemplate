/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class InventoryButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    [Header("References")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;



    [Header("Dependencies")]
    //[SerializeField] InventorySO _playersInventorySO;
    [SerializeField] PlayerInventory _playersInventory;
    [SerializeField] ItemDragAndDrop _dragAndDropReference;
    [SerializeField] InventoryUI1 _inventoryUIReference;
    [SerializeField] Transform toolTipPosition;
    [SerializeField] Image dragAndDropIcon;



    [Header("Tags - (Clothing/Ring/Potion/General)")]
    [SerializeField] string buttonTag;
    [SerializeField] ButtonTag buttonTagEnum;

    //private
    int myIndex;
    ToolTip toolTip;


    private void Awake()
    {
        toolTip = _inventoryUIReference.toolTip;

        if (toolTip == null)
        {
            Debug.LogError("No ToolTip Referenced");
        }
    }


    #region ------- SETTING BUTTONS ---------



    //////////////////////////////
    //SETTING / CLEARING DATA OF BUTTONS
    //////////////////////////////
    public void SetIndex(int index) //The index is needed for the OnSelect() Function. 
    {
        myIndex = index;
        dragAndDropIcon.gameObject.SetActive(false);
    }

    public void SetButtonSprites(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.GetIcon();

        if (slot.item.IsStackable())
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void SetButtonSprites(ItemSO item)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.GetIcon();
        text.gameObject.SetActive(false);
    }


    public void CleanButtons()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

    }

    public void SetButtonTag(ButtonTag tag)
    {
        buttonTagEnum = tag;
    }
    #endregion





    #region ------- SELECTING BUTTONS ---------



    //////////////////////////////
    //BUTTON ONCLICK EVENT / SELECTION
    //////////////////////////////
    public void OnSelect()  //MUST BE CALLED FROM THE BUTTONS ONCLICK EVENT
    {
        if(buttonTagEnum == ButtonTag.Null) {Debug.LogError("Button Tag Enum not set on " + gameObject.name); return;}
        if(myIndex >= _playersInventory.GetAvailableInventoryCount() && buttonTagEnum == ButtonTag.General) { return; }

        //PICKUP ITEM FROM BUTTON If button is not locked
        if (_dragAndDropReference.GetItemSlot().item == null)
        {
            if (buttonTagEnum == ButtonTag.Locked) {return;}

            SelectButton();       
        }
        //PLACE ITEM ONTO BUTTON
        else
        {
            if (_dragAndDropReference.GetItemSlot().item.GetButtonTag() == buttonTagEnum || buttonTagEnum == ButtonTag.General || buttonTagEnum == ButtonTag.Hotbar)
            {
                SelectButton();
            }
        }


        //DRAG AND DROP SETTINGS
        if(_dragAndDropReference.GetItemSlot().item != null) 
        {
            StoreDragAndDropIcon(_dragAndDropReference.GetItemSlot().item.GetIcon());
            ShowDragAndDropIcon(true);
        }
        else 
        {
            StoreDragAndDropIcon(null);
            ShowDragAndDropIcon(false);
        }


        _inventoryUIReference.ShowButtons();
        HideToolTip();
    }

    private void SelectButton()
    {
        if (buttonTagEnum == ButtonTag.General)
        {
            _dragAndDropReference.OnClick(_playersInventory.inventorySlots[myIndex]);
        }
        else if (buttonTagEnum == ButtonTag.Hotbar)
        {
            _dragAndDropReference.OnClick(_playersInventory.hotBarSlots[myIndex]);
        }
        else if (buttonTagEnum == ButtonTag.Misc)
        {
            _dragAndDropReference.OnClick(_playersInventory.miscSlots[myIndex]);
        }
    }

    public void OnSelectGarbage()
    {
        _dragAndDropReference.OnClickGarbage(_playersInventory.inventorySlots[myIndex]);
        _inventoryUIReference.ShowButtons();
    }
    #endregion





    #region ------- DRAG AND DROP ---------



    //////////////////////////////
    //DRAG AND DROP FUNCTIONS
    //////////////////////////////
    public void ShowDragAndDropIcon(bool active)
    {
        if (active) 
        {
            dragAndDropIcon.gameObject.SetActive(true);

            if (_dragAndDropReference.GetDragAndDropIcon() != null)
            {
                dragAndDropIcon.sprite = _dragAndDropReference.GetDragAndDropIcon();
            }
            else 
            {
                dragAndDropIcon.gameObject.SetActive(false);
            }
        }
        else 
        {
            dragAndDropIcon.gameObject.SetActive(false);
        }
    }

    private void StoreDragAndDropIcon(Sprite icon)
    {
        //if (icon == null) { _dragAndDropReference.itemIconImage.sprite = null; return; }

        _dragAndDropReference.SetDragAndDropIcon(icon);
    }
    #endregion





    #region ------- TOOLTIP ---------



    //////////////////////////////
    //TOOLTIP FUNCTIONS
    //////////////////////////////
    void ShowToolTip()
    {
            //InventorySO inventory = _playersInventory;
            PlayerInventory inventory = _playersInventory;

            if (buttonTagEnum == ButtonTag.General)
            {
                if (inventory.inventorySlots[myIndex].item == null) { HideToolTip(); return; }
                toolTip.ShowToolTipInfo(
                inventory.inventorySlots[myIndex].item.GetItemName(), 
                inventory.inventorySlots[myIndex].item.GetClass().ToString(), 
                inventory.inventorySlots[myIndex].item.GetItemDescription(),
                inventory.inventorySlots[myIndex].item.GetItemLevel(),
                toolTipPosition);
            }
            else if (buttonTagEnum == ButtonTag.Hotbar)
            {
                if (inventory.hotBarSlots[myIndex].item == null) { HideToolTip(); return; }
                toolTip.ShowToolTipInfo(
                inventory.hotBarSlots[myIndex].item.GetItemName(),
                inventory.hotBarSlots[myIndex].item.GetClass().ToString(),
                inventory.hotBarSlots[myIndex].item.GetItemDescription(),
                inventory.hotBarSlots[myIndex].item.GetItemLevel(),
                toolTipPosition);
            }
            else if (buttonTagEnum == ButtonTag.Misc)
            {
                if (inventory.miscSlots[myIndex].item == null) { HideToolTip(); return; }
                toolTip.ShowToolTipInfo(
                inventory.miscSlots[myIndex].item.GetItemName(),
                inventory.miscSlots[myIndex].item.GetClass().ToString(),
                inventory.miscSlots[myIndex].item.GetItemDescription(),
                inventory.miscSlots[myIndex].item.GetItemLevel(),
                toolTipPosition);
            }
        
    }

    void HideToolTip()
    {
        toolTip.HideToolTipInfo();
    }
    #endregion





    #region ------- EVENTS ---------



    //////////////////////////////
    //GAMEPAD / POINTER EVENTS
    //////////////////////////////
    public void OnSelect(BaseEventData eventData)
    {
        //if (_inventoryUIReference.IsInventoryToggled() == false) { return; }

        ShowToolTip();
        ShowDragAndDropIcon(true);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (_inventoryUIReference.IsInventoryToggled() == false) { return; }

        ShowToolTip();
        ShowDragAndDropIcon(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
        ShowDragAndDropIcon(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideToolTip();
        ShowDragAndDropIcon(false);
    }
    #endregion
}
