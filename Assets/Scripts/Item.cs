using DG.Tweening;
using UnityEngine;
using Zenject;

public class Item : MonoBehaviour
{
    [Inject] private InputSystem_DragAndDrop _scalingDown;
    [SerializeField] public string fruitName;
    
    public bool _isInGrid;

    private void OnEnable()
    {
        _scalingDown.ScaleDownObjectAction += OnScaleDowned;
    }

    private void OnDisable()
    {
        _scalingDown.ScaleDownObjectAction -= OnScaleDowned;
    }

    void OnScaleDowned(Transform clickedObject)
    {
        if (clickedObject == transform)
        {
            transform.DOScale(0.5f, 0.1f).SetEase(Ease.Linear);
        }
    }
    public bool PlacedInGrid()
    {
         _isInGrid = true;
         //Debug.Log("Is in grid ? "+ _isInGrid);
         return true;
    }
}
