using DG.Tweening;
using UnityEngine;
using Zenject;

public class Item : MonoBehaviour
{
    [SerializeField] public string fruitName;
    
    [Inject] private InputSystem_DragAndDrop _scalingDown;
    [Inject] private SignalBus _signalBus;

    private bool _isInGrid;

    public bool IsInGrid
    {
        get { return _isInGrid; }
        //set { _isInGrid = value; }
    }

    private void OnEnable()
    {
        
        _signalBus.Subscribe<TripleMatchSignals.ScaleDownObjectSignal>(OnScaleDowned);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<TripleMatchSignals.ScaleDownObjectSignal>(OnScaleDowned);
    }

    void OnScaleDowned(TripleMatchSignals.ScaleDownObjectSignal clickedObjectSignal)
    {
        if (clickedObjectSignal.ToDrag == transform)
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
