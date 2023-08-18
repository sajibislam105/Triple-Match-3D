using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;

    [SerializeField] private float _totalTime = 60.0f; //in seconds
    private float _currentTime;
    private float _totalTimeTakenToCompleteLevel;
    private bool _isLevelCompleted;
    private bool _isGamePaused;

    void Start()
    {
        _currentTime = _totalTime; // so that it gets reset every time level start again.
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<TripleMatchSignals.PlayNextUIButtonClickedSignal>(NextLevel);
        _signalBus.Subscribe<TripleMatchSignals.RestartUIButtonClickedSignal>(RestartLevel);
        _signalBus.Subscribe<TripleMatchSignals.GamePausedSignal>(PauseStatus);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<TripleMatchSignals.PlayNextUIButtonClickedSignal>(NextLevel);
        _signalBus.Unsubscribe<TripleMatchSignals.RestartUIButtonClickedSignal>(RestartLevel);
        _signalBus.Unsubscribe<TripleMatchSignals.GamePausedSignal>(PauseStatus);
    }
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Item") == null && !_isLevelCompleted)
        {
            //Invoke Level Complete UI
            _signalBus.Fire(new TripleMatchSignals.LevelCompleteSignal());
            
            _isLevelCompleted = true;
            if (_isLevelCompleted)
            {
                _signalBus.Fire(new TripleMatchSignals.SaveLevelSignal());
                _totalTimeTakenToCompleteLevel = _totalTime - _currentTime;
                //Debug.Log("Total time taken to complete the level: " + _totalTimeTakenToCompleteLevel
                float remainingTime = _totalTime - _totalTimeTakenToCompleteLevel;
                float percentRemaining = (remainingTime / _totalTime) * 100;
                
                //invoke star sequence
                _signalBus.Fire(new TripleMatchSignals.StarAchievedSignal()
                {
                    PercentRemaining =  percentRemaining
                });
            }
        }

        if (!_isGamePaused)
        {
            Timer();            
        }
    }

    void Timer()
    {
        if (_currentTime >= 0)
        {
            _currentTime -= Time.deltaTime;
            _signalBus.Fire(new TripleMatchSignals.RemainingTimeSendToUISignal()
            {
                CurrentTime = _currentTime
            });
        }
        else
        {
            if (!_isLevelCompleted)
            {
               //invoke on level failed
               _signalBus.Fire(new TripleMatchSignals.LevelFailedSignal());
            }
        }
    }

    void NextLevel()
    {
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex != lastSceneIndex)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);    
        }
        else
        {
            SceneManager.LoadScene(0); // 0 means landing page
        }
    }
    void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void PauseStatus(TripleMatchSignals.GamePausedSignal pauseStatus)
    {
        _isGamePaused = pauseStatus.IsPaused;
    }
    
}
