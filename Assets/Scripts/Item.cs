using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    private InputSystem_DragAndDrop _scalingDown;
    [SerializeField] public string fruitName;
    
    public bool _isInGrid;

    private void Awake()
    {
        _scalingDown = FindObjectOfType<InputSystem_DragAndDrop>();
    }

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
