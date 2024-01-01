using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] string interactableTag;
    [SerializeField] GameObject playerEntity;
    [SerializeField] GameObject interactionHUD;
    [SerializeField] HUDMessage HUDMessage;
    Interactable interactee;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactableTag))
        {
            if(other.GetComponent<Interactable>() == null) { Debug.LogError("Interactable Component is null on, " + other); }

            interactee = other.GetComponent<Interactable>();

            if(interactee.ShowInteractionOnHUD())
            {
                interactionHUD.SetActive(true);
            }

            if(interactee.HasOnEnterMessage)
            {
                HUDMessage.TriggerPopUpTextTemporary(interactee.GetOnEnterMessage());
            }

            interactee.TellInteractableAboutPlayer(playerEntity);
            interactee.OnEnter();
            OnEnter?.Invoke();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(interactableTag))
        {
            if(other.GetComponent<Interactable>() == null) { return; }
            other.GetComponent<Interactable>().OnStay();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(interactableTag))
        {
            if(interactee.HasOnEnterMessage)
            {
                HUDMessage.CancelPopup();
            }

            other.GetComponent<Interactable>().OnExit();
            //interactionHUD.SetActive(false);
            interactee = null;
            OnExit?.Invoke();
        }
    }

    public void Interact()
    {
        if(interactee != null)
        {
            interactee.Interact();
        }
    }

    public Interactable GetInteractee()
    {
        if(interactee == null) { return null; }
        return interactee;
    }
}
