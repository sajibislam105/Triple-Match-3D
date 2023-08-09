using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
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
    private Vector3 middlePosition;
    
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;

    private Dictionary<string, ItemInformation> _itemDictionary;
    private List<GridCellScript> _gridCellObjectsList;
    [SerializeField]private List<int> _indexList;
    private Vector3 _middleObjectPosition;

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
        //Debug.Log(_mergeParticleSystem.gameObject.name);
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
                _middleObjectPosition = GetMiddleObject(itemName);
                MergeObject(_itemDictionary[itemName].FruitScriptObjects,itemName, _middleObjectPosition);
            }
        }
        else
        {
            //Debug.Log("More than 7 object. No FruitScript found.");
        }
    }
    
    bool HasThreeSameObject(string itemName)
    {
        if (_itemDictionary[itemName].Count <= 3)
        {
            //Debug.Log("Three Same Objects");
            //fruitDictionary[FruitName].Count = 0;
            return true;
        }
        return false;
    }
    
    public Vector3 GetMiddleObject(string itemName)
    {
         _indexList = new List<int>();
        for (int i = 0; i < 7; i++) //grid list count 7
        {
            if (_gridCellObjectsList[i]._isOccupied &&  (_gridCellObjectsList[i].occupiedObject.fruitName == itemName))
            {
                Debug.Log("Occupied at index: " + i );
                _indexList.Add(i);
            }
            else
            {
                Debug.Log("not Occupied at index: " + i);
            }
        }
        Debug.Log("index list count: " + _indexList.Count);
        for (int j = 0; j < _indexList.Count; j++)
        {
            if (j == 1)
            {
               // Debug.Log("Reached here");
                Item middleItem = _itemDictionary[itemName].FruitScriptObjects[j];
                Vector3 middlePosition = middleItem.transform.position;
                return middlePosition;
            }
        }

        return Vector3.zero;
        /*int count = _itemDictionary[fruitName].Count;
        
        if (count >= 3) // If there are three items
        {
            int middleIndex = 1; // Middle item is the one at index 2 (0-based indexing)
            Item middleItem = _itemDictionary[fruitName].FruitScriptObjects[middleIndex];
            //Vector3 middlePosition = middleItem.transform.position;
           // Vector3 middlePosition = Vector3.zero;

            Item FirstItemOnGrid = _itemDictionary[fruitName].FruitScriptObjects[middleIndex-1];
            //Debug.Log(FirstItemOnGrid.gameObject.name + " 1 ");
            Item ThirdItemOnGrid = _itemDictionary[fruitName].FruitScriptObjects[middleIndex+1];
            //Debug.Log(ThirdItemOnGrid.gameObject.name+ " 3 ");

            float AB = Vector3.Distance(FirstItemOnGrid.transform.position, middleItem.transform.position); //first to middle
            float BC = Vector3.Distance(middleItem.transform.position, ThirdItemOnGrid.transform.position); // middle to third
            float AC = Vector3.Distance(FirstItemOnGrid.transform.position, ThirdItemOnGrid.transform.position); // first to third


            /*
            If (AB < AC) and (AB < BC), then position A is in the middle.
            If (BC < AB) and (BC < AC), then position B is in the middle.
            If (AC < AB) and (AC < BC), then position C is in the middle.
            
            #1#
            if ( (AB < AC) && (AB <BC ))
            {
              middlePosition = FirstItemOnGrid.transform.position;
              return middlePosition;
            }
            else if ( (BC < AB) && (BC <AC ))
            {
                middlePosition = middleItem.transform.position;
                return middlePosition;
            }
            else if( (AC < AB) && (AC <BC ))
            {
                middlePosition = ThirdItemOnGrid.transform.position;
                return middlePosition;
            }
            else
            {
                //equal
                return middleItem.transform.position;
            }

            //Debug.Log("Middle object Position: " + middlePosition + " Index number: " + middleIndex);
            //return middlePosition;
        }*/
    }
    
    
    private void MergeObject(List<Item> itemList, string receivedName, Vector3 mergePosition)
    {
        //Debug.Log("entered merge method");
        _audioSource.Play();
        //Debug.Log("merge Position: " + mergePosition + " in merge method.");
        foreach (var item in itemList)
        {
            //Debug.Log(fruitDictionary[fruitItem.fruitName].FruitScriptObjects.Count + "   count");
            if (_itemDictionary.ContainsKey(item.fruitName))
            { 
                //Debug.Log("Contains Key. " + "Received Name: " + ReceivedName);
                if (_itemDictionary[receivedName].Count >= 3 && mergePosition != Vector3.zero)
                {
                    //Debug.Log("Merging start");
                    //Debug.Log("Count = 3");

                    item.transform.DOMoveX(mergePosition.x, 0.75f).SetEase(Ease.InExpo).OnComplete((() =>
                    {
                        _mergeParticleSystem.transform.position = mergePosition; // setting the effect where the merge is happening
                        _audioSource.PlayOneShot(MergeAudioClip);
                        _mergeParticleSystem.Play();
                        item.transform.DOMoveY(2f, 1f).SetEase(Ease.Linear).WaitForCompletion();
                        item.transform.DOScale(0.75f, 0.75f).SetEase(Ease.Linear).OnComplete((() =>
                        {
                             item.transform.DOScale(0f, 0.1f).SetEase(Ease.OutBounce).OnComplete((() =>
                             {
                                 Transform fruitGameObject = item.transform;
                                 //Debug.Log("Destroying");
                                 _itemDictionary.Remove(item.fruitName);
                                 Destroy(fruitGameObject.gameObject, 0.1f);
                                 _itemDictionary[receivedName].Count = 0;
                             }));
                        }));    
                    }));
                }
                else
                {
                    //Debug.Log("not Merging");
                    //Debug.Log("Count is not 3, so exiting");
                }
            }
        }
    }

    //for debug purpose
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