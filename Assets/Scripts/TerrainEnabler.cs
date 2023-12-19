using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEnabler : MonoBehaviour
{
    void Awake()
    {
        GetComponent<TerrainCollider>().enabled = false;
        GetComponent<TerrainCollider>().enabled = true;
        Debug.Log("Terrain Collider Enabled");
    }
}
