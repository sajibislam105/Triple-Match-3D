using DG.Tweening;
using UnityEngine;

public class SlideIn : MonoBehaviour
{
    // Start is called before the first frame update
    private Card[] _cardsRectTransform;
    void Start()
    {
        _cardsRectTransform = GetComponentsInChildren<Card>();
        /*foreach (var rectTransform in _cardsRectTransform)
        {
         Debug.Log(rectTransform.gameObject.name);
        }*/

        for (int i = 0; i < _cardsRectTransform.Length; i++)
        {
            var cardComponent = _cardsRectTransform[i].gameObject.GetComponent<RectTransform>();
            cardComponent.DOAnchorPosX(25f * (i  * 8f) ,1f).SetEase(Ease.Unset);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
