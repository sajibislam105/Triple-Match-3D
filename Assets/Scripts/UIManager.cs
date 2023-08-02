using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    private AudioSource audioSource;
    private LevelManager levelManager;

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

    private bool _isPaused;
    private string _levelCount;

    private void Awake()
    {
        levelManager =FindObjectOfType<LevelManager>();
        _inputSystemDragAndDrop = FindObjectOfType<InputSystem_DragAndDrop>();
        audioSource = GetComponent<AudioSource>();
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

    /*private void OnLevelCountAction(int level)
    {
        _levelCount = level.ToString();
    }*/
    
    
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
}