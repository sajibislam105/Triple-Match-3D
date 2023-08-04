using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RemainingItemManager : MonoBehaviour
{
    [SerializeField]  private GameObject RemainingItemCardSlot;
    [SerializeField] private Transform DesiredParent;
    
    Dictionary<string, ItemDataForUI> AllItemData = new Dictionary<string, ItemDataForUI>();
    
    private void Awake()
    {
        UiITemCount();
    }

    void UiITemCount()
    {
        // Finding all game objects with the "Item" tag
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Item");

        // Loop through each item and collect their data
        foreach (GameObject gameObjectWithTag in gameObjectsWithTag)
        {
            // Assuming a script called "Item" attached to each item
            Item item = gameObjectWithTag.GetComponent<Item>();

            if (item != null)
            {
                // Get the fruit name from the item script
                string itemName = item.fruitName;

                // Check if the fruit name is already in the dictionary
                if (AllItemData.ContainsKey(itemName))
                {
                    // If the fruit name is already in the dictionary, increment the count
                    AllItemData[itemName].itemCounts++;
                    AllItemData[itemName].ItemScriptObjects.Add(item);
                }
                else
                {
                    // If the fruit name is not in the dictionary, add it with count 1
                    ItemDataForUI itemData = new ItemDataForUI();
                    itemData.itemCounts = 1;
                    itemData.ItemScriptObjects.Add(item);
                    AllItemData.Add(itemName, itemData);
                }
            }
        }
        GenerateItemCard();
        
    }

    void GenerateItemCard()
    {
        foreach (var KeyValuePair in AllItemData)
        {
            string remainingItemName = KeyValuePair.Key;
            int remainingItemCount = KeyValuePair.Value.itemCounts;

            GameObject itemCard = Instantiate(RemainingItemCardSlot, DesiredParent);
            Card card = itemCard.GetComponent<Card>();

            card.SetItemDetails(remainingItemName, remainingItemCount);
        }
    }
    
    public void RemoveItemFromDictionary(Item itemScript)
    {
        string itemName = itemScript.fruitName;
        //Debug.Log("Item name: "+itemName);
        
        if (AllItemData.ContainsKey(itemName))
        {
            // Decrementing the count
            AllItemData[itemName].itemCounts--;
            
            // Removing the item script from the list
            AllItemData[itemName].ItemScriptObjects.Remove(itemScript);

            int _itemCount = AllItemData[itemName].ItemScriptObjects.Count;
            //Debug.Log("Item Count:" + _itemCount);

            // If the count reaches 0, remove the fruit name from the dictionary
            if (AllItemData[itemName].itemCounts <= 0)
            {
                AllItemData.Remove(itemName);
            }
        }
        
        // Updating the UI after removing/merging the items
        UpdateCardUI();
    }
    
    private void UpdateCardUI()
    {
        // Clearing the full DesiredParent
        foreach (Transform child in DesiredParent)
        {
            Destroy(child.gameObject);
        }
        
        // Regenerating the Card UI based on the updated dictionary so the value of Items Card get updated.
        GenerateItemCard();
    }
    
     private void PrintDictionary()
    {
        foreach (var keyvalue in AllItemData)
        {
            var name = keyvalue.Key;
            var count = keyvalue.Value.itemCounts;
            var gameObjects = keyvalue.Value.ItemScriptObjects;
            
            //Debug.Log("Object Name: " + name + " Count: "+ count);
            foreach (var listItem in gameObjects)
            {
                Debug.Log("listItem:" + listItem);
            }
        }
    }
}

public class ItemDataForUI
{
    public int itemCounts = 0;

    public List<Item> ItemScriptObjects { get; set; }

    public ItemDataForUI() //constructor 
    {
        itemCounts = 0;
        ItemScriptObjects = new List<Item>();
    }
}
