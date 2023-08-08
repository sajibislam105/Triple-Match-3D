using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI remainingItemCountText,remainingItemNameText;

    public TextMeshProUGUI itemName
    {
        get { return remainingItemNameText; }
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void SetItemDetails(string itemName, int remainingItemCount)
    {
       remainingItemCountText.text = remainingItemCount.ToString();
        remainingItemNameText.text = itemName;
    }
}
