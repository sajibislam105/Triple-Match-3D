using UnityEngine;

public class GridCellScript : MonoBehaviour
{
    public bool _isOccupied;
    public GameObject occupiedObject;
    private Transform hitObject;
    
    // Start is called before the first frame update
    void Start()
    {
        occupiedObject = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            _isOccupied = false;
            occupiedObject = null;
        }
        else
        {
            occupiedObject = other.gameObject;
            _isOccupied = true;
        }
        //Debug.Log(gameObject.name + " Occupied by -> " + occupiedObject); 
    }
}
