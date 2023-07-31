using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    [SerializeField] private GridCellScript gridCellPrefab;
    [SerializeField] private int width;
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;
    
    public List<GridCellScript> GridCellObjectsList;
    private Dictionary<string, FruitData> fruitDictionary;
    
    
    void Awake()
    {
        GridCellObjectsList = new List<GridCellScript>(7);
        GenerateGrid();

        fruitDictionary = new Dictionary<string, FruitData>();
        audioSource = GetComponent<AudioSource>();
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
        var name = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position


        for (int x = 0; x < width; x++)
        {
            Vector3 worldPosition = new Vector3( (startX + x ) *( _cellSize + _spaceBetweenCell), 0,  0);
            GridCellScript gridCellObject = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            if (gridCellObject != null)
            {
                GridCellObjectsList.Add(gridCellObject);
            }
            gridCellObject.gameObject.transform.parent = gameObject.transform;
            gridCellObject.gameObject.name = "Grid Cell " + name;
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

    void OnDroppingObjectToCEll(FruitScript onCellFruitScript)
    {
        int _containedObject = 0;
        if (onCellFruitScript != null && _containedObject <= 7)
        {
            string fruitName = onCellFruitScript.fruitName;
            //Debug.Log("Fruit Name: " + fruitName);
            if (!fruitDictionary.ContainsKey(fruitName))
            {
                //Debug.Log("No existing "+ fruitName + ", so added one "+ fruitName +" to dictionary");
                _containedObject ++;
                FruitData fruitData = new FruitData();
                fruitData.Count = 1;
                fruitData.FruitScriptObjects.Add(onCellFruitScript);
                fruitDictionary.Add(fruitName,fruitData);
            }
            else
            {
                _containedObject ++;
                fruitDictionary[fruitName].Count++;
                //Debug.Log("Found " + fruitName + " in game object list, fruit count: " + fruitData.Count);
                fruitDictionary[fruitName].FruitScriptObjects.Add(onCellFruitScript);
            }
            //printDictionary();
            if (HasThreeSameObject(fruitName))
            {
                //send the list
                MergeObject(fruitDictionary[fruitName].FruitScriptObjects,fruitName);
            }
            
            //HasThreeSameObject(fruitName);
        }
        else
        {
            //Debug.Log("More than 7 object. No FruitScript found.");
        }
    }
    
    bool HasThreeSameObject(string FruitName)
    {
        //jei objecct add hoise oidar similar three object ase kina check korte hobe. then jodi thake taile oi 3 ta object merge korte pathabo. then destroy.
        //jei object ta add hoise oita hasthreeobject method e parameter e pathabo.
        // then oi object 3 ta hoise kina check korbe main Dictionary theke. jodi same hoy taile return true
        // ar na hoile return false;
        
        if (fruitDictionary[FruitName].Count >= 3)
        {
            Debug.Log("Three Same Objects");
            //fruitDictionary[FruitName].Count = 0;
            return true;
        }
        return false;
    }
    
    private void MergeObject(List<FruitScript> itemList, string ReceivedName)
    {
        Debug.Log("entered merge method");
        audioSource.Play();
        
        /*int[] indexes = new int[8];
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].fruitName == ReceivedName)
            {
                indexes[i] = i;
            }
        }*/
        
        foreach (var fruitItem in itemList)
        {
            //Debug.Log(fruitDictionary[fruitItem.fruitName].FruitScriptObjects.Count + "   count");
            if (fruitDictionary.ContainsKey(fruitItem.fruitName))
            { 
                Debug.Log("Contains Key. " + "Received Name: " + ReceivedName);
                
                if (fruitDictionary[ReceivedName].Count >= 3)
                {
                    Debug.Log("Count = 3");
                    fruitItem.transform.parent.DOMove(Vector3.zero, 0.4f).SetEase(Ease.Linear);
                    fruitItem.transform.parent.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
                    {
                        Transform FruitGameobjectParent = fruitItem.transform.parent;
                        Debug.Log("Destroying");
                        //fruitDictionary[ReceivedName].FruitScriptObjects.Remove(fruitItem);
                        fruitDictionary.Remove(fruitItem.fruitName);
                        Destroy(FruitGameobjectParent.gameObject, 0.1f);
                        fruitDictionary[ReceivedName].Count = 0;
                    }));
                }
                else
                {
                    Debug.Log("Count is not 3, so condition exit");
                }
                
            }
        }
        
       /* foreach (var fruitDictionaryValue in fruitDictionary.Values)
        {
            var fruitScriptObjects = fruitDictionaryValue.FruitScriptObjects;
            foreach (var fruitScript in fruitScriptObjects)
            {
                if (fruitScript.fruitName == Matchedname)
                {
                    fruitScript.transform.parent.DOMove(Vector3.zero, 0.4f).SetEase(Ease.Linear);
                    fruitScript.transform.parent.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
                    {
                        //printDictionary();
                        Transform FruitGameobjectParent = fruitScript.transform.parent;
                        Debug.Log("Destroying");
                        fruitDictionary.Remove(Matchedname);
                        //fruitDictionary.Remove(fruitItem.fruitName);
                        Destroy(FruitGameobjectParent.gameObject, 0.1f);
                    }));
                }
            }
        }*/
        
        
        
    }

    void printDictionary()
    {
        foreach (var keyvalue in fruitDictionary)
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