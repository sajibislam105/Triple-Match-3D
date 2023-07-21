using DG.Tweening;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Transform gridCellPrefab;
    [SerializeField] private int width;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var name = 0;
        float startX = -(width - 1) / 2f; // Calculate the starting X position

        for (int x = 0; x < width; x++)
        {
            Vector3 worldPosition = new Vector3( startX + x, 0,  0);
            Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
            obj.transform.parent = gameObject.transform;
            obj.name = "Grid Cell " + name;
            name++;
        }

        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        
        transform.Translate(transform.position.x,transform.position.y,0);
        transform.DOScale(0.75f, 0.01f).SetEase(Ease.Linear);
    }
    
}