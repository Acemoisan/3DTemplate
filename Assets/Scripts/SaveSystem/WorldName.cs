using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldName : MonoBehaviour
{
    [SerializeField] SaveSystem saveSystem;

    [SerializeField] TMP_InputField inputField;

    //[SerializeField] TextMeshProUGUI worldName;


    public void SubmitName() 
    {
        string customName = inputField.text;
        //worldName.text = customName;
        saveSystem.StartNewFile(customName);
    }
}
