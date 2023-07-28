using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    private FruitData fruitData;
    
    void Awake()
    {
        GridCellObjectsList = new List<GridCellScript>(7);
        GenerateGrid();
        
        fruitData = new FruitData();
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
      //  HasThreeSameObject();
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
        //Debug.Log("Action Called.");
        int _containedObject = 0;
        if (onCellFruitScript != null && _containedObject <= 7)
        {
            string fruitName1 = onCellFruitScript.fruitName;
            //Debug.Log("Fruit Name: " + fruitName);
            if (!fruitDictionary.ContainsKey(fruitName1))
            {
                //Debug.Log("No existing "+ fruitName1 + ", so added one "+ fruitName1 +" to dictionary");
                _containedObject ++;
                fruitData.Count = 1;
                fruitData.FruitScriptObjects.Add(onCellFruitScript);
                fruitDictionary.Add(fruitName1,fruitData);
            }
            else
            {
                _containedObject ++;
                fruitData.Count++;
                //Debug.Log("Found " + fruitName1 + " in game object list, fruit count: " + fruitData.Count);
                fruitData.FruitScriptObjects.Add(onCellFruitScript);
            }
            //printdictionary();
            HasThreeSameObject();
        }
        else
        {
            //Debug.Log("More than 7 object. No FruitScript found.");
        }
    }
    /*
     *  if (HasThreeSameObject())
                {
                    _containedObject -= 3;
                    MergeObject1(fruitData.FruitScriptObjects);
                    //StartCoroutine(MergeObject(fruitData.FruitScriptObjects));
                    //Debug.Log("three same objects matched, Merge coroutine started.");
                }
                else
                {
                    Debug.Log("matched not found yet");
                }
                
        foreach (var fruitData in fruitDictionary.Values)
        {
           // StopCoroutine(MergeObject(fruitData.FruitScriptObjects));
           // Debug.Log("coroutine stopped");
        }
     */
    bool HasThreeSameObject()
    {
        //jei objecct add hoise oidar similar three object ase kina check korte hobe. then jodi thake taile oi 3 ta object merge korte pathabo. then destroy.
        //jei object ta add hoise oita hasthreeobject method e parameter e pathabo.
        // then oi object 3 ta hoise kina check korbe main Dictionary theke. jodi same hoy taile return true
        // ar na hoile return false;
        /* Dictionary<List<FruitScript>, int> objectCount = new Dictionary<List<FruitScript>, int>();
 
         // Iterate through the fruitDictionary and count occurrences
         foreach (var fruitData in dictionary.Values)
         {
             if (objectCount.ContainsKey(fruitData.FruitScriptObjects))
             {
                 objectCount[fruitData.FruitScriptObjects]++;
             }
             else
             {
                 objectCount.Add(fruitData.FruitScriptObjects, 1);
             }
         }
         if (objectCount.Values.Count == 3)
         {
             Debug.Log("Three same object");
             
             return true;
         }
         return false;*/
        
        if (fruitData.Count == 3)
        {
            Debug.Log("Three Same Objects");
            MergeObject1(fruitData.FruitScriptObjects);
            return true;
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
                fruitDictionary.Remove(fruitItem.fruitName);
                //fruitData.Count = 0;
                Transform ParentTransform = fruitItem.transform.parent;
                Destroy(ParentTransform.gameObject,0.1f);
            }));
        }
        yield break;
    }
    
    private void MergeObject1(List<FruitScript> itemList)
    {
        Debug.Log("entered merge method");
        audioSource.Play();
        foreach (var fruitItem in itemList)
        {
        
            if (fruitDictionary.ContainsKey(fruitItem.fruitName))
            {
                Debug.Log("moving");
                printdictionary();
                fruitItem.transform.parent.DOMove(Vector3.zero, 0.4f).SetEase(Ease.Linear);
                fruitItem.transform.parent.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
                {
                    fruitDictionary.Remove(fruitItem.fruitName);
                    Transform FruitGameobjectParent = fruitItem.transform.parent; 
                    Debug.Log("destroying");
                    Destroy(FruitGameobjectParent.gameObject, 0.1f);
                    fruitData.FruitScriptObjects.Remove(fruitItem);
                }));
            }
        }
        
    }

    void printdictionary()
    {
        foreach (var keyvalue in fruitDictionary)
        {
            var name = keyvalue.Key;
            var count = keyvalue.Value.Count;
            var gameObjects = keyvalue.Value.FruitScriptObjects;
            
            Debug.Log("Object Name: " + name + " Count: "+ count);
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


