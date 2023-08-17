using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Inject] private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    [Inject] private LevelManager _levelManager;
    //[Inject] private CanvasGroup _canvasGroup;
    [Inject] private AudioSource _audioSource;

    [SerializeField]  private GameObject remainingItemCardSlot;
    [SerializeField] private Transform desiredParent;
    
    public Action RestartUIButtonClickedAction;
    public Action PlayNextUIButtonClickedAction;
    public Action<bool> GamePausedAction;
    
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private Button backButton;
    
    [SerializeField] private Button backToPlay;
    [SerializeField] private Button landingPage;
    [SerializeField] private Button close;

    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject levelFailed;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject stars;
    [SerializeField] private GameObject starsPlaceHolder;
    [SerializeField] private Image imageStar;
    [SerializeField] private Image imageStar1;
    [SerializeField] private Image imageStar2;

    [SerializeField] private TextMeshProUGUI instruction;
    
    Dictionary<string, ItemDataForUI> AllItemData = new Dictionary<string, ItemDataForUI>();
    
    private bool _isPaused;
    private string _levelCount;

    private void OnEnable()
    {
        _levelManager.RemainingTimeSendToUIAction += ReceivedRemainingTime;
        _levelManager.LevelCompleteAction += onLevelComplete;
        _levelManager.LevelFailedAction += onLevelFailed;
        _levelManager.StarAchievedAction += OnstarAchieved;

        _inputSystemDragAndDrop.InstructionStatusAction += OnInstructionStatusCall;
    }
    private void OnDisable()
    {
        _levelManager.RemainingTimeSendToUIAction -= ReceivedRemainingTime;
        _levelManager.LevelCompleteAction -= onLevelComplete;
        _levelManager.LevelFailedAction -= onLevelFailed;
        _levelManager.StarAchievedAction -= OnstarAchieved;
        
        _inputSystemDragAndDrop.InstructionStatusAction -= OnInstructionStatusCall;
    }

    private void Start()
    {
        imageStar.DOFade(0.1f, 0.1f);
        imageStar1.DOFade(0.1f, 0.1f);
        imageStar2.DOFade(0.1f, 0.1f);
        _levelCount = SceneManager.GetActiveScene().buildIndex.ToString();
        LevelText.text = "Level: " + _levelCount;
    }

    private void onLevelComplete()
    {
        levelComplete.SetActive(true);
        LevelText.enabled = false;
        TimerText.enabled = false;
        backButton.enabled = false;
        _inputSystemDragAndDrop.enabled = false;
        StarAppearingAnimatingFade();
    }
    
    public void onLevelFailed()
    {
        _inputSystemDragAndDrop.enabled = false;
        levelFailed.SetActive(true);
        LevelText.enabled = false;
        TimerText.text = "Time Finished";
        backButton.enabled = false;
    }

    private void ReceivedRemainingTime(float received_time)
    {
        int minutes = Mathf.FloorToInt(received_time / 60f);
        int seconds = Mathf.FloorToInt(received_time % 60f);
        
        if (received_time < 10f )
        {
            TimerText.color = Color.red;
        }
        string remainingTimeText= string.Format("{0:00}:{1:00}", minutes, seconds);
        TimerText.text = remainingTimeText;
    }

    public void OnPlayButtonClicked()
    {
        _audioSource.Play();
        //invoke next level
        PlayNextUIButtonClickedAction?.Invoke();
    }
    public void onRestartButtonClicked()
    {
        _audioSource.Play();
        //invoke restart
        RestartUIButtonClickedAction?.Invoke();
    }

    public void onBackButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = false;
        _audioSource.Play();
        LevelText.enabled = false;
        TimerText.enabled = false;
        //invoke pause
        _isPaused = true;
        GamePausedAction?.Invoke(_isPaused);
        pauseMenu.SetActive(true);
    }

    public void OnPauseMenuPlayButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = true;
        _audioSource.Play();
        LevelText.enabled = true;
        TimerText.enabled = true;
        //invoke pause
        _isPaused = false;
        GamePausedAction?.Invoke(_isPaused);
        pauseMenu.SetActive(false);
    }

    public void OnPauseMenuLandingPageButtonClicked()
    {
        _audioSource.Play();
        SceneManager.LoadScene(0); // 0 means Landing Page
    }
    
    public void OnPauseMenuCloseButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = true;
        _audioSource.Play();
        LevelText.enabled = true;
        TimerText.enabled = true;
        //invoke pause
        _isPaused = false;
        GamePausedAction?.Invoke(_isPaused);
        pauseMenu.SetActive(false);
    }

    private void OnstarAchieved(float percentageRemaining)
    {
        if (percentageRemaining > 50f)
        {
            //Debug.Log("3 star");
            imageStar.enabled = true;
            imageStar1.enabled = true;
            imageStar2.enabled = true;
        }
        else if (percentageRemaining > 25f)
        {
            //Debug.Log("2 star");
            imageStar.enabled = true;
            imageStar1.enabled = true;
            imageStar2.enabled = false;
        }
        else
        {
            //Debug.Log("1 star");
            imageStar.enabled = true;
            imageStar1.enabled = false;
            imageStar2.enabled = false;
        }
    }

    private void OnInstructionStatusCall()
    {
        instruction.enabled = false;
    }

    void StarAppearingAnimatingFade()
    {
        stars.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutSine).WaitForCompletion(); 
        Sequence sequence = DOTween.Sequence();
        sequence.Append(imageStar.DOFade(1f, 0.5f));
        sequence.Append(imageStar1.DOFade(1f, 0.5f));
        sequence.Append(imageStar2.DOFade(1f, 0.5f));
        stars.transform.DOScale(1f, 1.25f).SetEase(Ease.InSine).SetDelay(0.5f);
        //particle effects
    }

}


