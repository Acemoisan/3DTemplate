using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable
{
    [SerializeField] protected DialogConversationSO dialogConversationSO;

    public void StartConversationTrigger()
    {
        ConversationEvent();
    }

    protected virtual void ConversationEvent(){}
}
