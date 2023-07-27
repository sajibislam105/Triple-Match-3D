using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private int width;
    private float _cellSize = 1f;
    private float _spaceBetweenCell = 0.1f;
    
    public List<GameObject> GridCellGameObjectsList;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GridCellGameObjectsList = new List<GameObject>(7);
        GenerateGrid();
    }

    private void Update()
    {
        CheckingOccupancyOfCell();
        HasThreeSimilarObject();
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

    bool HasThreeSimilarObject()
    {
        int appleCounts = 0;
        int watermelonCounts = 0;
        int drinkCounts = 0;
        
        int containedObj = 0;
        
        List<GameObject> _appleList = new List<GameObject>();
        List<GameObject> _DrinkList = new List<GameObject>();
        List<GameObject> _WatermelonList = new List<GameObject>();

        foreach (var obj in GridCellGameObjectsList)
        {
            if (obj.GetComponent<GridCellScript>().occupiedObject != null && containedObj <= 7)
            {
                if (obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName == "Apple")
                {
                    containedObj += 1;
                    appleCounts += 1;
                    _appleList.Add(obj.GetComponent<GridCellScript>().occupiedObject);
                    //Debug.Log(obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName +" added " + appleCounts);
                }
                else if (obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName == "Drink")
                {
                    containedObj += 1;
                    drinkCounts += 1;
                    _DrinkList.Add(obj.GetComponent<GridCellScript>().occupiedObject);
                    //Debug.Log(obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName +" added " + drinkCounts);
                }
                else if (obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName == "Watermelon")
                {
                    containedObj += 1;
                    watermelonCounts += 1;
                   // _WatermelonList.Add(obj.GetComponent<GridCellScript>().occupiedObject);
                    Debug.Log(obj.GetComponent<GridCellScript>().occupiedObject.GetComponent<FruitScript>().fruitName +" added "+ watermelonCounts);
                }
                if (appleCounts == 3  || drinkCounts == 3 || watermelonCounts == 3 )
                {
                    if (appleCounts == 3)
                    {
                        StartCoroutine(MergeObject(_appleList));
                    }
                    else
                    {
                        StopCoroutine(MergeObject(_appleList));
                    }
                    if (drinkCounts == 3)
                    {
                        StartCoroutine(MergeObject(_DrinkList));
                    }
                    else
                    {
                        StopCoroutine(MergeObject(_DrinkList));
                    }
                    if (watermelonCounts == 3)
                    {
                        StartCoroutine(MergeObject((_WatermelonList)));
                    }
                    else
                    {
                        StopCoroutine(MergeObject((_WatermelonList)));
                    }
                    Debug.Log("Three same objects");
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator MergeObject(List<GameObject> itemList)
    {
        audioSource.Play();
        foreach (var fruitItem in itemList)
        {
            fruitItem.GetComponent<FruitScript>().transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
            fruitItem.GetComponent<FruitScript>().transform.DOScale(1f, 0.5f).SetEase(Ease.Linear).OnComplete((() =>
            {
                Destroy(fruitItem);
            }));
        }
        yield break;
    }
}