using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InputSystem_DragAndDrop : MonoBehaviour
{
    private Camera _camera;
    private AudioSource _audioSource;
    private RemainingItemManager _remainingItemManager;
    [SerializeField] private AudioClip objectDragAudioClip;
    
    private GridGenerator _gridGenerator;

    public Action<Transform> ScaleDownObjectAction;
    public Action<Item> ObjectDroppingOnCellAction;
    public Action InstructionStatusAction;

    private bool _isDragging;
    private Transform _toDrag;
    private Vector3 _newGridPosition;
    private Vector3 _oldPositionOfItem;
    
    [SerializeField] private List<bool> gridCellStatusList = new List<bool>();
    public List<bool> GridCellStatusList => gridCellStatusList;

    private void Awake()
    {
        _camera = Camera.main;
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _remainingItemManager = FindObjectOfType<RemainingItemManager>();
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        DragAndDrop();
    }
    
    private void DragAndDrop()
    {
        gridCellStatusList = _gridGenerator.CheckingOccupancyOfCell();
        Vector3 v3;
        if (Input.GetMouseButtonDown(0))
        {
            InstructionStatusAction?.Invoke(); // turning of the instruction
            
            RaycastHit? hitObject = CastRay();
            if (hitObject.HasValue && hitObject.Value.collider.CompareTag("Item") && !hitObject.Value.transform.GetComponent<Item>()._isInGrid)
            {
                _toDrag = hitObject.Value.transform;
                //float distance = hitObject.Value.transform.position.z - _camera.transform.position.z;
                _isDragging = true;
            }
            if (_toDrag != null)
            {
                _oldPositionOfItem = _toDrag.transform.position; //current position of object    
            }
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            v3 = _camera.ScreenToWorldPoint(v3);
            Vector3 offset = new Vector3(0, -5f, 0); // otherwise, it goes up when dragging
            _toDrag.position = v3 + offset;

            //Vector3 cameraToGridDirection = _toDrag.position - _camera.transform.position; //ray direction camera to _toDrag then _toDrag to Grid.
            var toDragObjectDownDirection = Vector3.down;
            Ray rayCastTowardsGrid = new Ray(_toDrag.transform.position, toDragObjectDownDirection);
            RaycastHit gridHit;
            Debug.DrawRay(rayCastTowardsGrid.origin, rayCastTowardsGrid.direction * 100f, Color.yellow);
            
            if (Physics.Raycast(rayCastTowardsGrid, out gridHit))
            {
                if (gridHit.transform.CompareTag("GridCell"))
                {
                    _newGridPosition = gridHit.transform.position;
                    //Debug.Log(gridHit.transform.name);
                }
            }
            else
            {
                _newGridPosition = _oldPositionOfItem; // if collider does not hit any grid then back to Old Grid
            }
            ScaleDownObjectAction?.Invoke(_toDrag);
        }

        if (Input.GetMouseButton(0) == false && _isDragging)
        {
            _isDragging = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_oldPositionOfItem != _newGridPosition && _toDrag != null)
            {
                for (int i = 0; i < _gridGenerator.GridCellObjectsList.Count; i++)   
                {
                    if (gridCellStatusList[i] == false) // false means not occupied
                    {
                        if (_gridGenerator.GridCellObjectsList[i].transform.position == _newGridPosition) // checking if gridCell and RayHit Cell are same.
                        {
                            var offsetPosition = new Vector3(0, 0.25f, 0);
                            _newGridPosition += offsetPosition; 
                            _toDrag.transform.position = _newGridPosition;
                            
                            Item toDragItem = _toDrag.GetComponent<Item>();
                            toDragItem.PlacedInGrid();
                            //invoke an action to add to the dictionary.
                            ObjectDroppingOnCellAction?.Invoke(toDragItem);
                            
                            _audioSource.PlayOneShot(objectDragAudioClip);
                            
                            if (_remainingItemManager != null)
                            {
                                _remainingItemManager.RemoveItemFromDictionary(toDragItem);    
                            }
                            _toDrag = null;
                        }
                    }
                    else
                    {
                        if (_toDrag !=null)
                        {
                           // Debug.Log("Cell Occupied");
                            _toDrag.transform.position = _oldPositionOfItem;                            
                        }
                    }
                }   
            }
            else
            {
                if (_toDrag !=null)
                {
                   //  Debug.Log("reached old position");
                    _toDrag.transform.DOScale(1f, 0.2f).SetEase(Ease.Linear);
                    _toDrag.transform.DOMove(_oldPositionOfItem,0.2f).SetEase(Ease.OutBack);                            
                }              
            }
        }
        gridCellStatusList = _gridGenerator.CheckingOccupancyOfCell();
    }
    private RaycastHit? CastRay()
    {
        var mousePositionInScreen = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePositionInScreen);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction * 50f,Color.red);
        if (Physics.Raycast(ray,out hit))
        {
            return hit;
        }
        return null;
    }
}