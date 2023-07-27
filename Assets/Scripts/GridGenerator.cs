using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private GridCellScript gridCellPrefab;
    [SerializeField] private int width;
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;
    
    public List<GridCellScript> GridCellObjectsList;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GridCellObjectsList = new List<GridCellScript>(7);
        GenerateGrid();
    }
    private void Update()
    {
        CheckingOccupancyOfCell();
        HasThreeSameObject();
    }
    private void GenerateGrid()
    {
        var name = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position


        for (int x = 0; x < width; x++)
        {
            Vector3 worldPosition = new Vector3( (startX + x ) *( _cellSize + _spaceBetweenCell), 0,  0);
            GridCellScript obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            if (obj != null)
            {
                GridCellObjectsList.Add(obj);
            }
            obj.gameObject.transform.parent = gameObject.transform;
            obj.gameObject.name = "Grid Cell " + name;
            name++;
        }
        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        transform.Translate(transform.position.x,transform.position.y,0);
        transform.DOScale(0.70f, 0.01f).SetEase(Ease.Linear);
    }

    public List<bool> CheckingOccupancyOfCell()
    {
        List<bool> occupancyStatusList = new List<bool>(10);
        foreach (GridCellScript gridCellGameObject in GridCellObjectsList)
        {
           occupancyStatusList.Add(gridCellGameObject._isOccupied);
        }
        return occupancyStatusList;
    }

    bool HasThreeSameObject()
    {
        int _containedObject = 0;
        Dictionary<string, FruitData> fruitDictionary = new Dictionary<string, FruitData>();
        foreach (GridCellScript gridCellGameObject in GridCellObjectsList) //DataType GridCell. that's why getting occupied object directly without Using GetComponent<>();
        {
            if (gridCellGameObject.occupiedObject != null && _containedObject <= 7)
            {
                string fruitName = gridCellGameObject.occupiedObject.fruitName;
                if (fruitDictionary.ContainsKey(fruitName))
                {
                    _containedObject += 1;
                    FruitData fruitData = fruitDictionary[fruitName];
                    fruitData.Count++;
                    fruitData.FruitScriptObjects.Add(gridCellGameObject.occupiedObject);
                    if (fruitData.Count == 3)
                    {
                        StartCoroutine(MergeObject(fruitData.FruitScriptObjects));
                        //Debug.Log("Three " + fruitName + " objects");
                        return true;
                    }
                }
                else
                {
                    _containedObject += 1;
                    FruitData fruitData = new FruitData();
                    fruitData.Count = 1;
                    fruitData.FruitScriptObjects.Add(gridCellGameObject.occupiedObject);
                    fruitDictionary.Add(fruitName,fruitData);
                }
            }
        }

        foreach (var fruitData in fruitDictionary.Values)
        {
            StopCoroutine(MergeObject(fruitData.FruitScriptObjects));
            //Debug.Log("code reached here down");
        }
        return false;
    }
    private IEnumerator MergeObject(List<FruitScript> itemList)
    {
        audioSource.Play();
        foreach (var fruitItem in itemList)
        {
            fruitItem.transform.parent.DOMove(Vector3.zero, 0.4f).SetEase(Ease.Linear);
            fruitItem.transform.parent.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
            {
                Destroy(fruitItem.gameObject,0.1f);
            }));
        }
        yield break;
    }
}

// Class for storing the FruitData
public class FruitData
{
    public int Count { get; set; }
    public List<FruitScript> FruitScriptObjects { get; set; }

    public FruitData() //constructor 
    {
        Count = 0;
        FruitScriptObjects = new List<FruitScript>();
    }
}


