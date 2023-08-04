using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI remainingItemCountText,remainingItemNameText;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>(); 
        _rectTransform.DORotate(new Vector3(0, 360, 0), 5f);
    }
    public void SetItemDetails(string itemName, int remainingItemCount)
    {
       remainingItemCountText.text = remainingItemCount.ToString();
        remainingItemNameText.text = itemName;
    }
}
