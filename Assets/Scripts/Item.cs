using System;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    private InputSystem_DragAndDrop scallingDown;
    private UIManager _uiManager;
    [SerializeField] public string fruitName;
    
    public bool _isInGrid;

    private void Awake()
    {
        scallingDown = FindObjectOfType<InputSystem_DragAndDrop>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        scallingDown.ScaleDownObjectAction += OnScaleDowned;
    }

    private void OnDisable()
    {
        scallingDown.ScaleDownObjectAction -= OnScaleDowned;
    }

    private void OnDestroy()
    {
        // Call the RemoveItemFromDictionary method in the ItemManager script
        if (_uiManager != null)
        {
            _uiManager.RemoveItemFromDictionary(this);
        }
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
         return true;
    }
}
