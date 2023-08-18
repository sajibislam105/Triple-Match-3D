using DG.Tweening;
using UnityEngine;

public class SlideIn : MonoBehaviour
{
    private Card[] _cardsRectTransform;
    void Start()
    {
        _cardsRectTransform = GetComponentsInChildren<Card>();

        for (int i = 0; i < _cardsRectTransform.Length; i++)
        {
            var cardComponent = _cardsRectTransform[i].gameObject.GetComponent<RectTransform>();
            cardComponent.DOAnchorPosX(25f * (i  * 8f) ,1f).SetEase(Ease.Unset);
        }
    }
}
