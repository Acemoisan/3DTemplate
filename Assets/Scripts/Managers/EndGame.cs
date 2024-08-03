using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGame : MonoBehaviour
{
    public UnityEvent OnEndGame;

    public void EndGameEvent()
    {
        if (OnEndGame != null)
        {
            OnEndGame.Invoke();
        }
    }
}
