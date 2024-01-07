using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Configuration")]
    public float textSpeed;

    [Header("Dependencies")]
    public UnityEngine.GameObject leftCharacter;
    public UnityEngine.GameObject rightCharacter;

    public TextMeshProUGUI leftCharacterName;
    public Image leftCharacterPortrait;

    public TextMeshProUGUI rightCharacterName;
    public Image rightCharacterPortrait;

    public TextMeshProUGUI dialogBox;
    //public List<GameObject> dialogChoiceHolders;

    private string _currentSentence;
    [SerializeField] DialogueManager dialogueManagerRef;
    DialogConversationSO currentConversation;


    public void StartConversation(
        string leftCharacterName,
        Sprite leftCharacterPortrait,
        string rightCharacterName,
        Sprite rightCharacterPortrait)
    {
        this.CleanUI();

        this.leftCharacterName.text = leftCharacterName;
        this.leftCharacterPortrait.sprite = leftCharacterPortrait;
        this.rightCharacterName.text = rightCharacterName;
        this.rightCharacterPortrait.sprite = rightCharacterPortrait;

        this.dialogBox.text = "";

        this.ToggleLeftCharacter(false);
        this.ToggleRightCharacter(false);
    }

    public void DisplaySentence(DialogConversationSO conversation, Sentence sentence)
    {
        this.currentConversation = conversation;
        if (sentence.dialogCharacter.displayName == leftCharacterName.text)
        {
            this.leftCharacterPortrait.sprite = sentence.dialogCharacter.portrait;
            this.ToggleLeftCharacter(true);
            this.ToggleRightCharacter(false);
        }
        else
        {
            this.rightCharacterPortrait.sprite = sentence.dialogCharacter.portrait;
            this.ToggleLeftCharacter(false);
            this.ToggleRightCharacter(true);
        }


        this._currentSentence = sentence.text;
        ClearOptionHolders();
        StartCoroutine(TypeCurrentSentence());

        // if (sentence.sentenceEvent != SentenceEvent.PlayerChoice)
        // {
        //     this._currentSentence = sentence.text;
        //     ClearOptionHolders();
        //     StartCoroutine(TypeCurrentSentence());
        // }
        // else
        // {
        //     StartCoroutine(ShowChoices(sentence));
        // }
    }

    public void EndConversation()
    {
        this.CleanUI();
    }

    public bool IsSentenceInProcess()
    {
        return this._currentSentence != null;
    }

    public void FinishDisplayingSentence()
    {
        StopAllCoroutines();
        this.dialogBox.text = this._currentSentence;
        this._currentSentence = null;
    }

    private IEnumerator TypeCurrentSentence()
    {
        this.dialogBox.text = "";

        foreach (char letter in this._currentSentence.ToCharArray())
        {
            this.dialogBox.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        this.dialogBox.text = this._currentSentence;
        this._currentSentence = null;
    }

//     private IEnumerator ShowChoices(Sentence sentence)
//     {
//         this.dialogBox.text = "";
//         ClearOptionHolders();
//         //dialogueManagerRef.ClearNextConversationListFromChoices();

//         yield return new WaitForSeconds(.25f);

//         for (int i = 0; i < sentence.choices.Length;)
//         {
//             string choiceText = sentence.choices[i].textChoice;

//             dialogChoiceHolders[i].SetActive(true);
//             dialogChoiceHolders[i].GetComponentInChildren<TextMeshProUGUI>().text = choiceText;
            

//             Button choiceHolderButton = dialogChoiceHolders[i].GetComponentInChildren<Button>();
//             choiceHolderButton.onClick.RemoveAllListeners();         
// choiceHolderButton.onClick.AddListener(() => dialogueManagerRef.NextSentence());
//             i++;
//         }

//         this._currentSentence = null;
//     }

    private void ClearOptionHolders()
    {
        // foreach (GameObject choiceHolder in dialogChoiceHolders)
        // {
        //     choiceHolder.SetActive(false);
        // }
    }

    private void CleanUI()
    {
        this.leftCharacterName.text = "";
        this.leftCharacterPortrait.sprite = null;
        this.rightCharacterName.text = "";
        this.rightCharacterPortrait.sprite = null;

        this.dialogBox.text = "";
        this._currentSentence = null;

        ClearOptionHolders();
    }

    private void ToggleLeftCharacter(bool status)
    {
        this.leftCharacter.SetActive(status);
    }

    private void ToggleRightCharacter(bool status)
    {
        this.rightCharacter.SetActive(status);
    }
}

