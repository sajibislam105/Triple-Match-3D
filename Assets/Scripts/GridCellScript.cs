using System;
using UnityEngine;

public class GridCellScript : MonoBehaviour
{
    public bool _isOccupied;
    public Item occupiedObject; // cause I need the fruit script mono behaviour not the whole game object 
    private Transform hitObject;

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
