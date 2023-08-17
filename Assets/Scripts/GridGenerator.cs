using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridGenerator : MonoBehaviour
{
    [Inject] private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    [Inject] private IMergeAble iMergeAble;
    [Inject] private DiContainer _container;

    [SerializeField] private GridCellScript gridCellPrefab;
    [SerializeField] private int width;
    private Vector3 middlePosition;
    
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;

    private Dictionary<string, ItemInformation> _itemDictionary;
    private List<GridCellScript> _gridCellObjectsList;
    [SerializeField]private List<int> _indexList;
    private Vector3 _middleObjectPosition;
    private string _currentItem;

    //Giving access to another class by Properties
    public List<GridCellScript> GridCellObjectsList => _gridCellObjectsList;

    public Dictionary<string, ItemInformation> ItemDictionary
    {
        get { return _itemDictionary; }
        set { _itemDictionary = value; }
    }

    void Awake()
    {
        _gridCellObjectsList = new List<GridCellScript>(7);
        GenerateGrid();
        _itemDictionary = new Dictionary<string, ItemInformation>();
    }

    private void OnEnable()
    {
        _inputSystemDragAndDrop.ObjectDroppingOnCellAction += OnDroppingObjectToGridCell;
    }

    private void OnDisable()
    {
        _inputSystemDragAndDrop.ObjectDroppingOnCellAction -= OnDroppingObjectToGridCell;
    }

    private void Update()
    {
        CheckingOccupancyOfCell();
    }
    private void GenerateGrid()
    {
        var number = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position
        for (int x = 0; x < width; x++)
        {
            Vector3 worldPosition = new Vector3( (startX + x ) *( _cellSize + _spaceBetweenCell), 0,  gameObject.transform.position.z);
            //GridCellScript gridCellObject = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            var gridCellObject = _container.InstantiatePrefab(gridCellPrefab.gameObject).GetComponent<GridCellScript>();
            var gridCellObjectTransform = gridCellObject.transform;
            gridCellObjectTransform.position = worldPosition;
            gridCellObjectTransform.rotation = Quaternion.identity;
            
            if (gridCellObject != null)
            {
                _gridCellObjectsList.Add(gridCellObject);
            }
            gridCellObject.gameObject.transform.parent = gameObject.transform;
            gridCellObject.gameObject.name = "Grid Cell " + number;
            number++;
        }
        transform.Translate(transform.position.x,transform.position.y,0);
        transform.DOScale(0.6f, 0.01f).SetEase(Ease.Linear);
    }

    public List<bool> CheckingOccupancyOfCell()
    {
        List<bool> occupancyStatusList = new List<bool>(10);
        foreach (GridCellScript gridCellGameObject in _gridCellObjectsList)
        {
           occupancyStatusList.Add(gridCellGameObject._isOccupied);
        }
        return occupancyStatusList;
    }

    void OnDroppingObjectToGridCell(Item onCellItem)
    {
        for (var i = 0; i < _inputSystemDragAndDrop.GridCellStatusList.Count; i++)
        {
            var gridCellStatus = _inputSystemDragAndDrop.GridCellStatusList[i];
            //Debug.Log($"grid cell status of {i} is {gridCellStatus}");
        }
        int containedObject = 0;
        if (onCellItem != null && containedObject <= 7)
        {
            string itemName = onCellItem.fruitName;
            //Debug.Log("Fruit Name: " + itemName);
            if (!_itemDictionary.ContainsKey(itemName))
            {
                //Debug.Log("No existing "+ itemName + ", so added one "+ itemName +" to dictionary");
                containedObject ++;
                ItemInformation itemInformation = new ItemInformation();
                itemInformation.Count = 1;
                itemInformation.FruitScriptObjects.Add(onCellItem);
                _itemDictionary.Add(itemName,itemInformation);
            }
            else
            {
                containedObject ++;
                _itemDictionary[itemName].Count++;
                //Debug.Log("Found " + itemName + " in game object list, fruit count: " + _itemDictionary[itemName].Count);
                _itemDictionary[itemName].FruitScriptObjects.Add(onCellItem);
            }
            
            //Till here occupancy status updated. Now Update Index List of occupied cells
            if (HasThreeSameObject(itemName))
            {
                //send the list
                _currentItem = itemName;
                Invoke(nameof(FoundThreeSamePostProcess),0.1f);
                
            }
        }
        else
        {
            //Debug.Log("More than 7 object. No FruitScript found.");
        }
        _middleObjectPosition = GetMiddleObject(onCellItem.fruitName);
    }
    private void FoundThreeSamePostProcess()
    {
        _middleObjectPosition = GetMiddleObject(_currentItem);
        iMergeAble.Merge(_itemDictionary[_currentItem].FruitScriptObjects,_currentItem, _middleObjectPosition);
    }
    
    bool HasThreeSameObject(string itemName)
    {
        if (_itemDictionary[itemName].Count == 3)
        {
            //Debug.Log("Three Same Objects");
            return true;
        }
        return false;
    }
    
    private Vector3 GetMiddleObject(string itemName)
    {
        _indexList = new List<int>();
        for (int i = 0; i < 7; i++) //grid list count 7
        {
            if (_gridCellObjectsList[i]._isOccupied &&  (_gridCellObjectsList[i].occupiedObject.fruitName == itemName))
            {
                //Debug.Log("Occupied at index: " + i );
                _indexList.Add(i);
            }
            else
            {
               // Debug.Log("not Occupied at index: " + i);
            }
        }
        for (int j = 0; j < _indexList.Count; j++)
        {
            if (j == 1)
            {
               var middleItem = _gridCellObjectsList[_indexList[j]];
                Vector3 middlePosition = middleItem.transform.position;
                return middlePosition;
            }
        }
        return Vector3.zero;
    }
}

// Class for storing the ItemData
public class ItemInformation
{
    public int Count { get; set; }
    public List<Item> FruitScriptObjects { get; set; }

    public ItemInformation() //constructor 
    {
        Count = 0;
        FruitScriptObjects = new List<Item>();
    }
}