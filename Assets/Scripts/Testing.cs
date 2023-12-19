using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    void Start()
    {
        DamagePopup.Create(this.transform.position, 300);
    }

}
