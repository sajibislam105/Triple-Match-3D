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
       // _rectTransform.DORotate(new Vector3(0, 180, 0), 1f).SetEase(ease: Ease.InCirc).SetLoops(-1,LoopType.Yoyo);
    }

    public void SetItemDetails(string itemName, int remainingItemCount)
    {
       remainingItemCountText.text = remainingItemCount.ToString();
        remainingItemNameText.text = itemName;
    }
}
