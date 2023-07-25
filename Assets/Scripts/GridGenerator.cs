using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
   // private GridCellScript _gridCellScript;
    
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private int width;
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;
    
    public List<GameObject> GridCellGameObjectsList;
    
    void Awake()
    {
        GridCellGameObjectsList = new List<GameObject>(7);
        GenerateGrid();
    }

    private void Update()
    {
        CheckingOccupancyOfCell();
    }

    private void GenerateGrid()
    {
        var name = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position


        for (int x = 0; x < width; x++)
        {
            Vector3 worldPosition = new Vector3( (startX + x ) *( _cellSize + _spaceBetweenCell), 0,  0);
            GameObject obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            if (obj != null)
            {
                //_gridCellScript = obj.GetComponent<GridCellScript>();
                GridCellGameObjectsList.Add(obj);
            }
            obj.transform.parent = gameObject.transform;
            obj.name = "Grid Cell " + name;
            name++;
        }
        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        
        transform.Translate(transform.position.x,transform.position.y,0);
        transform.DOScale(0.75f, 0.01f).SetEase(Ease.Linear);
    }

    public List<bool> CheckingOccupancyOfCell()
    {
        List<bool> occupancyStatusList = new List<bool>(10);
        foreach (GameObject gridCellGameObject in GridCellGameObjectsList)
        {
            GridCellScript gridCellScript = gridCellGameObject.GetComponent<GridCellScript>();
            if(gridCellScript != null)
            {
                occupancyStatusList.Add(gridCellScript._isOccupied);
            }
        }
        return occupancyStatusList;
    }
}