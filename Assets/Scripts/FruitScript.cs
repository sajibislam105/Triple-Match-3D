using DG.Tweening;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] private InputSystem_DragAndDrop _ScallingDown;

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
            transform.DOScale(0.25f, 0.1f).SetEase(Ease.Linear);
        }
    }
}
