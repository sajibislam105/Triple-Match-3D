using UnityEngine;

public class GridCellScript : MonoBehaviour
{
    private bool _isOccupied;
    private Item occupiedObject; // cause I need the fruit script mono behaviour not the whole game object

    public Item OccupiedObject
    {
        get { return occupiedObject; }
        private set { occupiedObject = value; }
    }
    public bool IsOccupied
    {
        get { return _isOccupied; }
        private set { _isOccupied = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            occupiedObject = other.GetComponent<Item>();
            //Debug.Log("Occupied");
            _isOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isOccupied = false;
        occupiedObject = null;
        //Debug.Log("Not Occupied");
    }

}
