using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public GameObject toolTip;
    public TextMeshProUGUI toolTipItemName;
    public TextMeshProUGUI toolTipItemLevel;
    public TextMeshProUGUI toolTipClassName;
    public TextMeshProUGUI toolTipShortDescription;


    public void ShowToolTipInfo(string itemName, string toolClass, string description, int level, Transform position)
    {
        toolTipItemName.text = itemName;
        toolTipItemLevel.text = "Level: " + level.ToString();
        toolTipClassName.text = toolClass;
        toolTipShortDescription.text = description;
        toolTip.SetActive(true);

        gameObject.transform.position = position.position;
    }

    public void HideToolTipInfo()
    {
        toolTipItemName.text = null;
        toolTipClassName.text = null;
        toolTipShortDescription.text = null;
        toolTip.SetActive(false);
    }
}
