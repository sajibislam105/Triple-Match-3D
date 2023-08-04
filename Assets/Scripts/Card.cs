using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingItemCountText,remainingItemNameText;
    
    public void SetItemDetails(string itemName, int remainingItemCount)
    {
       remainingItemCountText.text = remainingItemCount.ToString();
        remainingItemNameText.text = itemName;
    }
}
