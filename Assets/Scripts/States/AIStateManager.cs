using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateManager : MonoBehaviour
{
    [SerializeField] public AIState currentState;
    [SerializeField] protected bool stayInStartState;


    void Update()
    {
        HandleCurrentState();
    }

    public void SetState(AIState state)
    {
        currentState = state;
    }

    public void StayInState(bool stay)
    {
        stayInStartState = stay;
    }

    public abstract void HandleCurrentState();

}
