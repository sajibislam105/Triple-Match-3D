using DG.Tweening;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private InputSystem_DragAndDrop _ScallingDown;
    [SerializeField] public string fruitName;
    
    public bool _isInGrid;
    
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
            transform.parent.DOScale(0.5f, 0.1f).SetEase(Ease.Linear);
        }
    }
    public bool PlacedInGrid()
    {
         _isInGrid = true;
         return true;
    }
}
