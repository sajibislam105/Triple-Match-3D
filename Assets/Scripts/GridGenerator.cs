using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private AudioSource _audioSource;
    private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    private UIManager _uiManager;
    private ParticleSystem _mergeParticleSystem;
    [SerializeField] private AudioClip MergeAudioClip; 

    [SerializeField] private GridCellScript gridCellPrefab;
    [SerializeField] private int width;
    
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;

    private Dictionary<string, ItemInformation> _itemDictionary;
    private List<GridCellScript> _gridCellObjectsList;

    //Giving access to another class by Properties
    public List<GridCellScript> GridCellObjectsList
    {
        get {return _gridCellObjectsList;}
    }

    
    void Awake()
    {
        _inputSystemDragAndDrop = FindObjectOfType<InputSystem_DragAndDrop>();
        _gridCellObjectsList = new List<GridCellScript>(7);
        GenerateGrid();

        _itemDictionary = new Dictionary<string, ItemInformation>();
        _audioSource = GetComponent<AudioSource>();
        _mergeParticleSystem = GetComponentInChildren<ParticleSystem>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        _inputSystemDragAndDrop.ObjectDroppingOnCellAction += OnDroppingObjectToCEll;
    }

    private void OnDisable()
    {
        _inputSystemDragAndDrop.ObjectDroppingOnCellAction -= OnDroppingObjectToCEll;
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
            GridCellScript gridCellObject = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            if (gridCellObject != null)
            {
                _gridCellObjectsList.Add(gridCellObject);
            }
            gridCellObject.gameObject.transform.parent = gameObject.transform;
            gridCellObject.gameObject.name = "Grid Cell " + number;
            number++;
        }
        //gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        //gameObject.transform.position = new Vector3(0f, 0.5f, -3.5f);
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

    void OnDroppingObjectToCEll(Item onCellItem)
    {
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
            //printDictionary();
            if (HasThreeSameObject(itemName))
            {
                //send the list
                MergeObject(_itemDictionary[itemName].FruitScriptObjects,itemName);
            }
        }
        else
        {
            //Debug.Log("More than 7 object. No FruitScript found.");
        }
    }
    
    bool HasThreeSameObject(string fruitName)
    {
        if (_itemDictionary[fruitName].Count >= 3)
        {
            //Debug.Log("Three Same Objects");
            //fruitDictionary[FruitName].Count = 0;
            return true;
        }
        return false;
    }
    
    private void MergeObject(List<Item> itemList, string receivedName)
    {
        //Debug.Log("entered merge method");
        _audioSource.Play();
        foreach (var item in itemList)
        {
            //Debug.Log(fruitDictionary[fruitItem.fruitName].FruitScriptObjects.Count + "   count");
            if (_itemDictionary.ContainsKey(item.fruitName))
            { 
                //Debug.Log("Contains Key. " + "Received Name: " + ReceivedName);
                if (_itemDictionary[receivedName].Count >= 3)
                {
                    //Debug.Log("Count = 3");
                    item.transform.DOMove(Vector3.zero, 0.4f).SetEase(Ease.Linear).OnComplete((() =>
                    {
                        _mergeParticleSystem.transform.position = Vector3.zero;
                        _audioSource.PlayOneShot(MergeAudioClip);
                        _mergeParticleSystem.Play();
                    }));
                    item.transform.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
                    {
                        Transform fruitGameobject = item.transform;
                        //Debug.Log("Destroying");
                        _itemDictionary.Remove(item.fruitName);
                        Destroy(fruitGameobject.gameObject, 0.1f);
                        _itemDictionary[receivedName].Count = 0;
                    }));
                    
                }
                else
                {
                    //Debug.Log("Count is not 3, so exiting");
                }
            }
        }
       
        
    }

    //for debug purposes
    void PrintDictionary()
    {
        foreach (var keyvalue in _itemDictionary)
        {
            var name = keyvalue.Key;
            var count = keyvalue.Value.Count;
            var gameObjects = keyvalue.Value.FruitScriptObjects;
            
            //Debug.Log("Object Name: " + name + " Count: "+ count);
            foreach (var listItem in gameObjects)
            {
                Debug.Log("listItem:" + listItem);
            }
        }
    }
}

// Class for storing the FruitData
[Serializable]
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