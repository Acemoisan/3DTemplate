using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHudSize : MonoBehaviour
{
    [SerializeField] RectTransform _hotbarRectTransform;
    [SerializeField] RectTransform _healthRectTransform;
    [SerializeField] RectTransform _timeRectTransform;
    [SerializeField] RectTransform _itemPickedUpRectTextTransform;
    [SerializeField] RectTransform _activeItemRectTextTransform;

    

    [Header("Small Sizes")]
    [SerializeField] Vector2 _hotbarSmallSize;
    [SerializeField] Vector2 _healthSmallSize;
    [SerializeField] Vector2 _timeSmallSize;
    [SerializeField] Vector2 _itemPickedUpSmallSize;
    [SerializeField] Vector2 _activeItemSmallSize;


    [Header("Medium Sizes")]
    [SerializeField] Vector2 _hotbarMediumSize;
    [SerializeField] Vector2 _healthMediumSize;
    [SerializeField] Vector2 _timeMediumSize;
    [SerializeField] Vector2 _itemPickedUpMediumSize;
    [SerializeField] Vector2 _activeItemMediumSize;


    [Header("Large Sizes")]
    [SerializeField] Vector2 _hotbarLargeSize;
    [SerializeField] Vector2 _healthLargeSize;
    [SerializeField] Vector2 _timeLargeSize;
    [SerializeField] Vector2 _itemPickedUpLargeSize;
    [SerializeField] Vector2 _activeItemLargeSize;



    void SetSmallSizes()
    {
        _hotbarRectTransform.localScale = _hotbarSmallSize;
        _healthRectTransform.localScale = _healthSmallSize;
        _timeRectTransform.localScale = _timeSmallSize;
        _itemPickedUpRectTextTransform.localScale = _itemPickedUpSmallSize;
        _activeItemRectTextTransform.localScale = _activeItemSmallSize;
    }

    void SetMediumSizes()
    {
        _hotbarRectTransform.localScale = _hotbarMediumSize;
        _healthRectTransform.localScale = _healthMediumSize;
        _timeRectTransform.localScale = _timeMediumSize;
        _itemPickedUpRectTextTransform.localScale = _itemPickedUpMediumSize;
        _activeItemRectTextTransform.localScale = _activeItemMediumSize;
    }

    void SetLargeSizes()
    {
        _hotbarRectTransform.localScale = _hotbarLargeSize;
        _healthRectTransform.localScale = _healthLargeSize;
        _timeRectTransform.localScale = _timeLargeSize;
        _itemPickedUpRectTextTransform.localScale = _itemPickedUpLargeSize;
        _activeItemRectTextTransform.localScale = _activeItemLargeSize;
    }

    public void SetSize(CanvasSize canvasSize)
    {
        switch (canvasSize)
        {
            case CanvasSize.Small:
                SetSmallSizes();
                break;
            case CanvasSize.Medium:
                SetMediumSizes();
                break;
            case CanvasSize.Large:
                SetLargeSizes();
                break;
            default:
                break;
        }
    }
}
