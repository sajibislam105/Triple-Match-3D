using UnityEngine;

public class GridCellScript : MonoBehaviour
{
    public bool _isOccupied;
    public FruitScript occupiedObject; // cause I need the fruit script mono behaviour not the whole game object 
    private Transform hitObject;
    
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
            occupiedObject = other.GetComponent<FruitScript>();
            _isOccupied = true;
        }
        //Debug.Log(gameObject.name + " Occupied by -> " + occupiedObject); 
    }

    private void OnTriggerExit(Collider other)
    {
        _isOccupied = false;
        occupiedObject = null;
    }
}
