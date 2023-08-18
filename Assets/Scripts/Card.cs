using TMPro;
using UnityEngine;
using Zenject;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingItemCountText,remainingItemNameText;
    
    [Inject] private RectTransform _rectTransform;

    public TextMeshProUGUI itemName
    {
        get { return remainingItemNameText; }
    }
    public void SetItemDetails(string itemName, int remainingItemCount)
    {
       remainingItemCountText.text = remainingItemCount.ToString();
        remainingItemNameText.text = itemName;
    }
}
