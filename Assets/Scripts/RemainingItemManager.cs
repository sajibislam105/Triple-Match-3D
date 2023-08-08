using System.Collections.Generic;
using UnityEngine;

public class RemainingItemManager : MonoBehaviour
{
    [SerializeField]  private GameObject RemainingItemCardSlot;
    [SerializeField] private Transform DesiredParent;
    [SerializeField] private float DestroyAfterSeconds = 1f;
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

            // Checking if a card for this item already exists in the UI
            Card existingCard = FindCardForItem(remainingItemName);

            if (existingCard != null)
            {
                // Updating the value on the existing card
                existingCard.SetItemDetails(remainingItemName, remainingItemCount);
            }
            else
            {
                // Creating a new card
                GameObject itemCard = Instantiate(RemainingItemCardSlot, DesiredParent);
                Card card = itemCard.GetComponent<Card>();

                card.SetItemDetails(remainingItemName, remainingItemCount);
            }
        }
    }

    private Card FindCardForItem(string itemName)
    {
        // going through all existing cards and find the one with a matching item name
        foreach (Transform child in DesiredParent)
        {
            Card card = child.GetComponent<Card>();

            if (card != null && card.itemName.text == itemName)
            {
                return card;
            }
        }

        return null; // No card found for the specified item
    }

    
    public void RemoveItemFromDictionary(Item itemScript)
    {
        string itemName = itemScript.fruitName;

        if (AllItemData.ContainsKey(itemName))
        {
            // Decrementing the count
            AllItemData[itemName].itemCounts--;
            // Removing the item script from the list
            AllItemData[itemName].ItemScriptObjects.Remove(itemScript);

            int _itemCount = AllItemData[itemName].ItemScriptObjects.Count;

            // If the count reaches 0, remove the fruit name from the dictionary and destroy the card
            if (_itemCount <= 0)
            {
                // Finding the card for this item and update its value before destroying
                Card existingCard = FindCardForItem(itemName);
                if (existingCard != null)
                {
                    existingCard.SetItemDetails(itemName, _itemCount);
                }
                
                AllItemData.Remove(itemName);

                // Finding and destroying the card associated with this item
                Card cardToRemove = FindCardForItem(itemName);
                if (cardToRemove != null)
                {
                    Destroy(cardToRemove.gameObject,DestroyAfterSeconds);
                }
            }
            else
            {
                // Find the card for this item and update its value
                Card existingCard = FindCardForItem(itemName);
                if (existingCard != null)
                {
                    existingCard.SetItemDetails(itemName, _itemCount);
                }
            }
        }
    }
    
     private void PrintDictionary()
    {
        foreach (var keyValuePair in AllItemData)
        {
            var name = keyValuePair.Key;
            var count = keyValuePair.Value.itemCounts;
            var gameObjects = keyValuePair.Value.ItemScriptObjects;
            
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
