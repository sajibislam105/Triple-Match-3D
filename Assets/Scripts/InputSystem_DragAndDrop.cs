using DG.Tweening;
using UnityEngine;

public class InputSystem_DragAndDrop : MonoBehaviour
{
    private Camera _camera;

    private bool _isDragging;
    private Transform _toDrag;
    private float _distance;
    private Vector3 _offset;

    private Vector3 _newGridPosition;
    private Vector3 _oldGridPosition;
    
    private void Awake()
    {
        _camera = Camera.main;
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
            if (hitObject.HasValue && hitObject.Value.collider.CompareTag("Fruit"))
            {
                _toDrag = hitObject.Value.transform;
                _distance = hitObject.Value.transform.position.z - _camera.transform.position.z;
                //v3 = hit.Value.point;
                //_offset = _toDrag.position - v3;
                _isDragging = true;
            }
            _oldGridPosition = _toDrag.transform.position; //current position of object
        }
        
        if (Input.GetMouseButton(0) && _isDragging)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance - 2f);
            v3 = _camera.ScreenToWorldPoint(v3);
            //_toDrag.position = v3 + _offset;
            _toDrag.position = v3;

            Vector3 cameraToGridDirection = _toDrag.position - _camera.transform.position;  //ray direction camera to _toDrag then _toDrag to Grid.
            Ray rayCastTowardsGrid = new Ray(_toDrag.transform.position, cameraToGridDirection);
            RaycastHit GridHit;
            //Debug.DrawRay(rayCastTowardsGrid.origin,rayCastTowardsGrid.direction * 100f ,Color.yellow);
            if (Physics.Raycast(rayCastTowardsGrid,out GridHit))
            {
                if(GridHit.transform.CompareTag("GridCell"))
                {
                    _newGridPosition= GridHit.transform.position;
                    //Debug.Log(GridHit.transform.name);
                }
            }
            else
            {
                _newGridPosition = _oldGridPosition;  // if collider does not hit any grid then back to Old Grid
                
            }
        }
        
        if (Input.GetMouseButton(0) == false && _isDragging)
        {
            _isDragging = false;
        }
        
        if (Input.GetMouseButtonUp(0)) 
        {
            if (_oldGridPosition != _newGridPosition)
            {
                _toDrag.transform.position = _newGridPosition;
            }
            else
            {
                _toDrag.transform.position = _oldGridPosition;
            }
        }
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
