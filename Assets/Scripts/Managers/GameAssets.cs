using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    static GameAssets _instance;

    public static GameAssets instance
    {
        get
        {
            if (_instance == null) _instance = (Instantiate(Resources.Load("GameAssets")) as UnityEngine.GameObject).GetComponent<GameAssets>();
            return _instance;
        }
    }


    //References to Assets
    public Transform damagePopup;
}
