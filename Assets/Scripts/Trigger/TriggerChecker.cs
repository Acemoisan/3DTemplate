using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerChecker : MonoBehaviour
{
    // [Header("Extra config")]
    // [SerializeField] string validTag;
    // [SerializeField] LayerMask _layerMask;



    [Header("Player Trigger Events")]
    //public UnityEvent onInteract;
    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;
    [SerializeField] GameObject player;



    public GameObject GetPlayer()
    {
        //if(_interactee == null) Debug.Log("Interactee (Player) is null");
        return player == null ? null : player;
    }

    public void SetPlayer(GameObject interactee)
    {
        player = interactee;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(collision.name);
            SetPlayer(collision.gameObject);

            if(onEnter != null)
            {
                onEnter.Invoke();
            }


            // if(this._interactable.enemy) return;
            // this.interactionRequestEvent.Raise((_interactable != null));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetPlayer(null);

            if(onExit != null)
            {
                onExit.Invoke();
            }


            // if(this._interactable.enemy) return;
            // this.interactionRequestEvent.Raise((_interactable != null));
        }

        //this.interactionRequestEvent.Raise((_interactable != null));
    }
}
