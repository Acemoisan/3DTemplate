using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerAnimation playerAnimation;
    public UnityEvent OnPrimaryAttack;
    public UnityEvent OnSecondaryAttack;
    public UnityEvent EmptyHandEvent;


    
    public void PrimaryAttack(InputAction.CallbackContext value)
    {
		if (value.started)
        {
            OnPrimaryAttack?.Invoke();

            //Check if player has an active item
            if(playerInventory.GetActiveItem() != null)
            {

                //Check if item has an action
                if(playerInventory.GetActiveItem().ItemAction != null)
                {

                    //pass all necessary params to the tool / hit function. Grab any necessary functions / values
                    playerInventory.GetActiveItem().ItemAction.OnPrimaryAction(playerInventory.GetActiveItem(), playerInventory); //out int _itemDurability

                    playerInventory.GetActiveItem().ItemAction.OnItemUsed(playerInventory.GetActiveItem(), playerInventory);
                }
                else 
                {
                    Debug.Log(playerInventory.GetActiveItem() + " Item Action is null");
                }



                //Play Animation Based on item's AnimationString enum
                if(playerInventory.GetActiveItem().GetAnimationString != AnimationString.NoAnimation)
                {
                    playerAnimation.SetTrigger(playerInventory.GetActiveItem().GetAnimationString.ToString());
                    Debug.Log(playerInventory.GetActiveItem() + " Animation String is " + playerInventory.GetActiveItem().GetAnimationString.ToString());
                }
                else 
                {
                    Debug.Log(playerInventory.GetActiveItem() + " Animation String is null");
                }
            }
            else
            {
                EmptyHandEvent?.Invoke();
            }
        }
    }

    public void SecondaryAttack(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnSecondaryAttack?.Invoke();
        }
    }
}
