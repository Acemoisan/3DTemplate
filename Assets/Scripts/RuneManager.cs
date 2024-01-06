using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    [SerializeField] List<GameObject> runeObjects;
    [SerializeField] TimeManagerSO timeManager;
    [SerializeField] int numbersOfRunesNeededToDestroy;
    [SerializeField] int runesDestroyed;
    public GameEvent runeDestroyedEvent;
    public GameEvent allRunesDestroyedEvent;
    public GameEvent allRunesDestroyedAfter6Event;



    void Start()
    {
        numbersOfRunesNeededToDestroy = runeObjects.Count;
    }

    public void DestroyRune()
    {
        runesDestroyed++;

        if(runesDestroyed >= numbersOfRunesNeededToDestroy)
        {
            if(timeManager.hour >= 6)
            {
                allRunesDestroyedAfter6Event.Raise();
            }
            else 
            {
                allRunesDestroyedEvent.Raise();
            }
            return;
        }

        runeDestroyedEvent.Raise();
    }
}
