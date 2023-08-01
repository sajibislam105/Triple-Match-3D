using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InputSystem_DragAndDrop : MonoBehaviour
{
    private Camera _camera;
    private AudioSource audioSource; 
    [SerializeField] private AudioClip objectDragAudioClip;
    
    private GridGenerator gridGenerator;

    public Action<Transform> ScaleDownObjectAction;
    public Action<ItemScript> ObjectDroppingOnCellAction; 

    private bool _isDragging;
    private Transform _toDrag;
    private float _distance;

    private Vector3 _newGridPosition;
    private Vector3 _oldPositionOfItem;
    
    [SerializeField] private List<bool> gridCellStatusList = new List<bool>();
    
    private void Awake()
    {
        _camera = Camera.main;
        gridGenerator = FindObjectOfType<GridGenerator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        DragAndDrop();
    }
    
    private void DragAndDrop()
    {
        Vector3 v3;
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit? hitObject = CastRay();
            if (hitObject.HasValue && hitObject.Value.collider.CompareTag("Item") && !hitObject.Value.transform.GetComponent<ItemScript>()._isInGrid)
            {
                _toDrag = hitObject.Value.transform;
                _distance = hitObject.Value.transform.position.z - _camera.transform.position.z;
                _isDragging = true;
            }

            if (_toDrag != null)
            {
                _oldPositionOfItem = _toDrag.transform.position; //current position of object    
            }
            
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance - 2f);
            v3 = _camera.ScreenToWorldPoint(v3);
            _toDrag.position = v3;

            Vector3 cameraToGridDirection =
                _toDrag.position -
                _camera.transform.position; //ray direction camera to _toDrag then _toDrag to Grid.
            Ray rayCastTowardsGrid = new Ray(_toDrag.transform.position, cameraToGridDirection);
            RaycastHit GridHit;
            Debug.DrawRay(rayCastTowardsGrid.origin, rayCastTowardsGrid.direction * 100f, Color.yellow);
            
            if (Physics.Raycast(rayCastTowardsGrid, out GridHit))
            {
                if (GridHit.transform.CompareTag("GridCell"))
                {
                    _newGridPosition = GridHit.transform.position;
                    //Debug.Log(GridHit.transform.name);
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
                for (int i = 0; i < gridGenerator.GridCellObjectsList.Count; i++)   
                {
                    if (gridCellStatusList[i] == false) // false means not occupied
                    {
                        if (gridGenerator.GridCellObjectsList[i].transform.position == _newGridPosition) // checking if gridCell and RayHit Cell are same.
                        {
                            var offsetPosition = new Vector3(0, 0, -0.5f);
                            _newGridPosition += offsetPosition; 
                            _toDrag.transform.position = _newGridPosition;
                            
                            _toDrag.GetComponent<ItemScript>().PlacedInGrid();
                            //invoke an action to add to the dictionary.
                            ObjectDroppingOnCellAction?.Invoke(_toDrag.GetComponent<ItemScript>());
                            audioSource.PlayOneShot(objectDragAudioClip);
                            _toDrag = null;
                        }
                    }
                    else
                    {
                        if (_toDrag !=null)
                        {
                            _toDrag.transform.position = _oldPositionOfItem;                            
                        }
                    }
                }   
            }
            else
            {
                if (_toDrag !=null)
                {
                    _toDrag.transform.DOScale(1f, 0.2f).SetEase(Ease.Linear);
                    _toDrag.transform.DOMove(_oldPositionOfItem,0.2f).SetEase(Ease.OutBack);                            
                }              
            }
        }
        gridCellStatusList = gridGenerator.CheckingOccupancyOfCell();
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
