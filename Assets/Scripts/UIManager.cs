using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField]  private GameObject remainingItemCardSlot;
    [SerializeField] private Transform desiredParent;

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
    
    [Inject] private InputSystem_DragAndDrop _inputSystemDragAndDrop;
    [Inject] private AudioSource _audioSource;
    [Inject] private SignalBus _signalBus;

    Dictionary<string, ItemDataForUI> AllItemData = new Dictionary<string, ItemDataForUI>();

    private bool _isPaused;
    private string _levelCount;

    private void OnEnable()
    {
        _signalBus.Subscribe<TripleMatchSignals.RemainingTimeSendToUISignal>(ReceivedRemainingTime);
        _signalBus.Subscribe<TripleMatchSignals.LevelCompleteSignal>(onLevelComplete);
        _signalBus.Subscribe<TripleMatchSignals.LevelFailedSignal>(onLevelFailed);
        _signalBus.Subscribe<TripleMatchSignals.StarAchievedSignal>(OnstarAchieved);
        
        _signalBus.Subscribe<TripleMatchSignals.InstructionStatusSignal>(OnInstructionStatusCall);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<TripleMatchSignals.RemainingTimeSendToUISignal>(ReceivedRemainingTime);
        _signalBus.Unsubscribe<TripleMatchSignals.LevelCompleteSignal>(onLevelComplete);
        _signalBus.Unsubscribe<TripleMatchSignals.LevelFailedSignal>(onLevelFailed);
        _signalBus.Unsubscribe<TripleMatchSignals.StarAchievedSignal>(OnstarAchieved);
        
        _signalBus.Unsubscribe<TripleMatchSignals.InstructionStatusSignal>(OnInstructionStatusCall);
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

    private void ReceivedRemainingTime(TripleMatchSignals.RemainingTimeSendToUISignal received_time)
    {
        int minutes = Mathf.FloorToInt(received_time.CurrentTime / 60f);
        int seconds = Mathf.FloorToInt(received_time.CurrentTime % 60f);
        
        if (received_time.CurrentTime < 10f )
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
        _signalBus.Fire(new TripleMatchSignals.PlayNextUIButtonClickedSignal());
    }
    public void onRestartButtonClicked()
    {
        _audioSource.Play();
        //invoke restart
        _signalBus.Fire(new TripleMatchSignals.RestartUIButtonClickedSignal());
    }

    public void onBackButtonClicked()
    {
        _inputSystemDragAndDrop.enabled = false;
        _audioSource.Play();
        LevelText.enabled = false;
        TimerText.enabled = false;
        //invoke pause
        _isPaused = true;
        _signalBus.Fire(new TripleMatchSignals.GamePausedSignal()
        {
            IsPaused = _isPaused
        });
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
        _signalBus.Fire(new TripleMatchSignals.GamePausedSignal()
        {
            IsPaused = _isPaused
        });
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
        _signalBus.Fire(new TripleMatchSignals.GamePausedSignal()
        {
            IsPaused = _isPaused
        });
        pauseMenu.SetActive(false);
    }

    private void OnstarAchieved(TripleMatchSignals.StarAchievedSignal starAchievedSignal)
    {
        var percentageRemaining = starAchievedSignal.PercentRemaining;
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


