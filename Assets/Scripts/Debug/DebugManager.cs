using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;

    public bool debug_drawAimRays {get; private set;} = false; 
    public bool debug_drawColliderBoxes = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDebugDrawAimRays(bool set)
    {
        debug_drawAimRays = set;
    }
}
