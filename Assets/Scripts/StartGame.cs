using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
    [SerializeField] ItemDatabase itemDatabase;
    public UnityEvent OnStartGame;

    public void StartGameEvent()
    {
        if (OnStartGame != null)
        {
            OnStartGame.Invoke();
        }

        ItemNameSuggestor.itemDatabase = itemDatabase;
    }
}
