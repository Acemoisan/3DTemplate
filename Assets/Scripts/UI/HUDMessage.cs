using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class HUDMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popUpTextRef;
    string popUpText;
    [SerializeField] float popUpTextSpeed;

    public UnityEvent onPopUpActivated;
    public UnityEvent onPopUpDeactivated;
    bool triggered = false;

    public void TriggerPopUpTextTemporary(string text)
    {
        Debug.Log("TriggerPopUpTextTemporary");
        SetUPPopup(text, 5, true);
    }

    public void TriggerPopUpTextTemporaryTwo(string text)
    {
        CancelPopup();
        Debug.Log("TriggerPopUpTextTemporary");
        SetUPPopup(text, 10, true);
    }

    public void TriggerPopUpText(string text, bool triggerTypeWriterEffect = false)//, string buttonOneText, string buttonTwoText)
    {
        SetUPPopup(text, 10000, triggerTypeWriterEffect);
    }

    void SetUPPopup(string text, float cancelTimer, bool triggerTypeWriterEffect = false)
    {
        if(triggered) { return; }
        triggered = true;
        popUpText = text;

        if (this.onPopUpActivated != null) { this.onPopUpActivated.Invoke(); }

        if(triggerTypeWriterEffect) { StartCoroutine(TypeCurrentSentence()); }
        else { ShowPopUpText(); }

        StartCoroutine(CancelPopupTimer(cancelTimer));
    }

    void ShowPopUpText()
    {
        this.popUpTextRef.text = popUpText;
    }
    public void CancelPopup()
    {
        StopAllCoroutines();
        triggered = false;
        triggeredSentence = false;
        popUpTextRef.text = "";
        if (this.onPopUpDeactivated != null) { this.onPopUpDeactivated.Invoke(); }
    }

    IEnumerator CancelPopupTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        CancelPopup();
    }
    // public void NextSentence()
    // {
    //     if (IsSentenceInProcess())
    //     {
    //         FinishDisplayingSentence();
    //         return;
    //     }
    //     else if (IsSentenceInProcess() == false)
    //     {
    //         ShowChoices();
    //         return;
    //     }
    // }

    // public bool IsSentenceInProcess()
    // {
    //     return this.popUpText != null;
    // }

    // public void FinishDisplayingSentence()
    // {
    //     StopAllCoroutines();
    //     triggeredSentence = false;
    //     this.popUpTextRef.text = this.popUpText;
    //     this.popUpText = null;
    // }    

    bool triggeredSentence = false;
    private IEnumerator TypeCurrentSentence()
    {
        if (triggeredSentence) { yield break; }
        triggeredSentence = true;

        this.popUpTextRef.text = "";

        foreach (char letter in this.popUpText.ToCharArray())
        {
            this.popUpTextRef.text += letter;
            yield return new WaitForSeconds(popUpTextSpeed);
        }

        this.popUpTextRef.text = this.popUpText;
        this.popUpText = null;
        triggeredSentence = false;
    }
}
