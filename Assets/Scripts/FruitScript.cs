using DG.Tweening;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private InputSystem_DragAndDrop _ScallingDown;
    [SerializeField] public string fruitName;
    
    public bool isInGrid;
    
    private void OnEnable()
    {
        _ScallingDown.ScaleDownObjectAction += OnScaleDowned;
    }

    private void OnDisable()
    {
        _ScallingDown.ScaleDownObjectAction -= OnScaleDowned;
    }
    
    void OnScaleDowned(Transform clickedObject)
    {
        if (clickedObject == transform)
        {
         if (fruitName == "Watermelon")
         {
             transform.DOScaleX(0.24f, 0.1f).SetEase(Ease.Linear);
              transform.DOScaleY(0.30f, 0.1f).SetEase(Ease.Linear);
              transform.DOScaleZ(1.74f, 0.1f).SetEase(Ease.Linear);
         }
         else
         {
             transform.DOScale(0.30f, 0.1f).SetEase(Ease.Linear);
         }
        }
    }

    public bool PlacedInGrid()
    {
         isInGrid = true;
         return true;
    }
}
