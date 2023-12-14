/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */
using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;



public class Interactable : MonoBehaviour
{
    [Header("Action Events")]
    public UnityEvent onAbilityPressed;
    public UnityEvent onInteract;
    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;

    [Header("Config")]
    [SerializeField] bool showInteractionOnHUD = true;
    public bool ShowInteractionOnHUD() { return showInteractionOnHUD; }
    [SerializeField] string onEnterMessage;
    public string GetOnEnterMessage() { return onEnterMessage; }
    [HideInInspector] public bool HasOnEnterMessage;

    
    [SerializeField] GameObject _player;
    bool doNotTriggerInteractionUIButton;
    bool interactedTrigger = false;



    void Start()
    {
        _player = null;
        HasOnEnterMessage = onEnterMessage != "" ? true : false;
    }


    public void AbilityPressed()
    {
        if(onAbilityPressed != null)
        {
            if(interactedTrigger == false) 
            {
                onAbilityPressed.Invoke();

                interactedTrigger = true;
                StartCoroutine(ResetInteracted());
            }        
        }
    }


    public void Interact()
    {
        if(onInteract != null)
        {   
            if(interactedTrigger == false) 
            {
                interactedTrigger = true;

                StartCoroutine(ResetInteracted());
                Debug.Log("Interact");
                onInteract.Invoke();
            }
        }
    }


    IEnumerator ResetInteracted()
    {
        yield return new WaitForSeconds(0.1f);
        interactedTrigger = false;
    }


    public void OnEnter()
    {
        interactedTrigger = false;
        if(onEnter != null)
        {
            onEnter.Invoke();
        }
    }


    public void OnStay()
    {
        if(onStay != null)
        {
            onStay.Invoke();
        }
    }


    public virtual void OnExit()
    {
        if(onExit != null)
        {
            onExit.Invoke();
        }
    }


    public void TellInteractableAboutPlayer(GameObject interactee)
    {
        _player = interactee;
    }


    public GameObject GetPlayer()
    {
        if(_player == null) {Debug.LogError("Player not found"); return null; }
        return _player;
    }
}
