using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackBoxController : MonoBehaviour
{
    public UnityEvent EnableColliderEvent;
    public UnityEvent DisableColliderEvent;


    public void EnableCollider()
    {
        EnableColliderEvent.Invoke();
    }

    public void DisableCollider()
    {
        DisableColliderEvent.Invoke();
    }
}
