using TMPro;
using UnityEngine;
using Zenject;

public class Card : MonoBehaviour
{
    [Inject] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI remainingItemCountText,remainingItemNameText;

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
