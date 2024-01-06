using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneStone : Interactable
{

    //[SerializeField] string message;
    public void TriggerMessage(bool trigger)
    {
        if(GetPlayer() == null) { return; }

        if(trigger)
        {
            //GetPlayer().GetComponentInChildren<HUDMessage>().TriggerPopUpText(message, true);   
        }
        else 
        {
            GetPlayer().GetComponentInChildren<HUDMessage>().CancelPopup();
        }
    }
}
