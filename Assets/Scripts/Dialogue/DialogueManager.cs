

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class DialogueManager : MonoBehaviour
{
    [Header("Dependencies")]
    public DialogueUI dialogUI;


    [Header("Action Events")]
    public UnityEvent onConversationStarted;
    public UnityEvent onConversationEnded;


    private Queue<Sentence> sentences; //sentences stored from each conversation
    private DialogConversationSO currentConversation; //saved on the start of the conversation



    private void Start()
    {
        this.sentences = new Queue<Sentence>();
    }



    public void StartConversation(DialogConversationSO conversation) //Can be called from anywhere
    {
        if(conversation == null) { Debug.LogError("You must assign a conversation to " + this); return; }
        
        currentConversation = conversation;

        Debug.Log("Starting conversation with " + conversation.name);

        if (this.sentences.Count != 0) { return; }

        foreach (var sentence in conversation.sentences)
        {
            this.sentences.Enqueue(sentence);
        }


        this.dialogUI.StartConversation(
            leftCharacterName: conversation.leftCharacter.displayName,
            leftCharacterPortrait: conversation.leftCharacter.portrait,
            rightCharacterName: conversation.rightCharacter.displayName,
            rightCharacterPortrait: conversation.rightCharacter.portrait
        );


        // Trigger the OnConversationStarted Event list
       // I was using this to turn the Dialogue Menu + HUD Menu On and Off
        if (this.onConversationStarted != null) { this.onConversationStarted.Invoke(); }


        this.NextSentence();
    }


    public void NextSentence() // *** This is also called by player Input. When player presses "interact" 
    {

        if (this.dialogUI.IsSentenceInProcess())
        {
            this.dialogUI.FinishDisplayingSentence();
            return;
        }

        if (this.sentences.Count == 0)
        {
            this.EndConversation();
            return;
        }

        var sentence = this.sentences.Dequeue();

        this.dialogUI.DisplaySentence(currentConversation, sentence);    
    }    




    public void EndConversation() //CLEAR UI AND TURN OFF CANVAS
    {
        this.dialogUI.EndConversation();

        if (this.onConversationEnded != null)
        {
            this.onConversationEnded.Invoke();
        }
    }

    public bool IsDialogueFinished()
    {
        return !dialogUI.gameObject.activeSelf;
    }
}

