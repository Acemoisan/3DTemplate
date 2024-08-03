using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : DialogueTrigger
{
    protected override void ConversationEvent()
    {
        if(GetPlayer().GetComponentInChildren<DialogueManager>() == null) { return; }
        GetPlayer().GetComponentInChildren<DialogueManager>().StartConversation(dialogConversationSO);
    }
}
