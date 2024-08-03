using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InventoryTab : MonoBehaviour
{
    public UnityEvent OnTabSelect;
    public void OnTabUpdate()
    {
        OnTabSelect.Invoke();
    }
}
