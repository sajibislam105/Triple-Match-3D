using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    private AudioSource audioSource;
    private LevelManager levelManager;

    [SerializeField]  private GameObject RemainingItemCardSlot;
    [SerializeField] private Transform DesiredParent;
    
    public Action RestartUIButtonClickedAction;
    public Action PlayNextUIButtonClickedAction;
    public Action<bool> GamePausedAction;
    
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private Button backButton;
    
    [SerializeField] private Button backToPlay;
    [SerializeField] private Button landingPage;
    [SerializeField] private Button close;

    [SerializeField] private GameObject _LevelComplete;
    [SerializeField] private GameObject _LevelFailed;
    [SerializeField] private GameObject _Pausemenu;

    [SerializeField] private GameObject Star;
    [SerializeField] private Image _imagestar;
    [SerializeField] private Image _imagestar1;
    [SerializeField] private Image _imagestar2;

    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private TextMeshProUGUI itemNameText;
    
    Dictionary<string, ItemDataForUI> AllItemData = new Dictionary<string, ItemDataForUI>();
    
    private bool _isPaused;
    private string _levelCount;

    private void Awake()
    {
        levelManager =FindObjectOfType<LevelManager>();
        _inputSystemDragAndDrop = FindObjectOfType<InputSystem_DragAndDrop>();
        audioSource = GetComponent<AudioSource>();
        UiITemCount();
    }
    private void OnEnable()
    {
        levelManager.RemainingTimeSendToUIAction += ReceivedRemainingTime;
        levelManager.LevelCompleteAction += onLevelComplete;
        levelManager.LevelFailedAction += onLevelFailed;
        levelManager.StarAchievedAction += OnstarAchieved;
        
       
    }
    private void OnDisable()
    {
        levelManager.RemainingTimeSendToUIAction -= ReceivedRemainingTime;
        levelManager.LevelCompleteAction -= onLevelComplete;
        levelManager.LevelFailedAction -= onLevelFailed;
        levelManager.StarAchievedAction -= OnstarAchieved;
        
        
    }

    void Start()
    {
        _levelCount = SceneManager.GetActiveScene().buildIndex.ToString();
        LevelText.text = "Level: " + _levelCount;
    }

    private void onLevelComplete()
    {
        _LevelComplete.SetActive(true);
        LevelText.enabled = false;
        TimerText.enabled = false;
        backButton.enabled = false;
        _inputSystemDragAndDrop.enabled = false;
    }
    
    private void onLevelFailed()
    {
        _inputSystemDragAndDrop.enabled = false;
        _LevelFailed.SetActive(true);
        LevelText.enabled = false;
        TimerText.text = "Time Finished";
        backButton.enabled = false;
    }

    private void ReceivedRemainingTime(string received_time)
    {
        TimerText.text = received_time;
    }
    
    
    public void OnPlayButtonClicked()
    {
        audioSource.Play();
        //invoke next level
        PlayNextUIButtonClickedAction?.Invoke();
    }
    public void onRestartButtonClicked()
    {
        audioSource.Play();
        //invoke restart
        RestartUIButtonClickedAction?.Invoke();
    }

    public void onBackButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = false;
        audioSource.Play();
        LevelText.enabled = false;
        TimerText.enabled = false;
        //invoke pause
        _isPaused = true;
        GamePausedAction?.Invoke(_isPaused);
        _Pausemenu.SetActive(true);
    }

    public void OnPauseMenuPlayButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = true;
        audioSource.Play();
        LevelText.enabled = true;
        TimerText.enabled = true;
        //invoke pause
        _isPaused = false;
        GamePausedAction?.Invoke(_isPaused);
        _Pausemenu.SetActive(false);
    }

    public void OnPauseMenuLandingPageButtonClicked()
    {
        audioSource.Play();
        SceneManager.LoadScene(0); // 0 means Landing Page
    }
    
    public void OnPauseMenuCloseButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = true;
        audioSource.Play();
        LevelText.enabled = true;
        TimerText.enabled = true;
        //invoke pause
        _isPaused = false;
        GamePausedAction?.Invoke(_isPaused);
        _Pausemenu.SetActive(false);
    }

    void OnstarAchieved(float percentageRemaining)
    {
        if (percentageRemaining > 50f)
        {
            //Debug.Log("3 star");
            _imagestar.enabled = true;
            _imagestar1.enabled = true;
            _imagestar2.enabled = true;
        }
        else if (percentageRemaining > 25f)
        {
            //Debug.Log("2 star");
            _imagestar.enabled = true;
            _imagestar1.enabled = true;
            _imagestar2.enabled = false;
        }
        else
        {
            Debug.Log("1 star");
            _imagestar.enabled = true;
            _imagestar1.enabled = false;
            _imagestar2.enabled = false;
        }
    }
    
    /*a method to calculate the all the items in the object and how many of them using dictionary
    dictionary<string, items>; items consist object and count
     find the object using tag.
    items  have a public variable name fruit name. from which I can get the key of dictionary.
    */
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
    
     void printDictionary()
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
