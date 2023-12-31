using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PickedUpItemUI : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] GameObject itemUIHolder;
    [SerializeField] Image _itemIcon;
    [SerializeField] TextMeshProUGUI _itemCountText;
    [SerializeField] TextMeshProUGUI _itemNameText;
    // [SerializeField] Sprite plusIcon;
    // [SerializeField] Color energyColor;
    // [SerializeField] Color manaColor;
    // [SerializeField] Color healthColor;


    [Header("Configurables")]
    [SerializeField] int showUITime;


    Vector3 cachedPos;
    private string _lastItemName = "";
    private int _lastItemCount = 0;


    void Start()
    {
        cachedPos = itemUIHolder.transform.position;
        ClearUI();
    }

    public void FillUI(Sprite icon, string itemName, int itemCount)
    {
        StopAllCoroutines();
        StartCoroutine(FillUIDelay(icon, itemName, itemCount));
    }

    // public void ShowIncreaseOfPlayerStat(string itemName)
    // {
    //     StopAllCoroutines();
    //     StartCoroutine(FillUIDelay(plusIcon, itemName, 0));
    //     _itemIcon.color = energyColor;
    //     _itemCountText.text = " ";
    // }

    private IEnumerator FillUIDelay(Sprite icon, string itemName, int itemCount)
    {
        itemUIHolder.SetActive(true);
        _itemNameText.text = itemName;
        itemUIHolder.transform.position = cachedPos;
        _itemIcon.sprite = icon;


        if (_lastItemName == itemName)
        {
            _lastItemCount += itemCount;
        }
        else
        {
            _lastItemCount = itemCount;
        }
        
        _itemCountText.text = _lastItemCount.ToString();
        _lastItemName = itemName;

        yield return new WaitForSeconds(showUITime);
        ClearUI();
    }

    public void ClearUI()
    {
        _itemIcon.sprite = null;
        _itemNameText.text = null;
        _itemCountText.text = null;
        _lastItemCount = 1;
        itemUIHolder.SetActive(false);
    }

    void Update()
    {
        float moveYSpeed = 6f;
        itemUIHolder.transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
    }
}
