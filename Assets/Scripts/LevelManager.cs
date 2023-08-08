using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UIManager _uiManager;

    public Action LevelCompleteAction;
    public Action LevelFailedAction;
    public Action<float> RemainingTimeSendToUIAction;
    public Action<float> StarAchievedAction;

    [SerializeField] private float _totalTime = 60.0f; //in seconds
    private float _currentTime;
    private float _totalTimeTakenToCompleteLevel;
    
    private bool _isLevelCompleted;
    private bool _isGamePaused;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        _currentTime = _totalTime; // so that it gets reset every time level start again.
    }
    
    private void OnEnable()
    {
        _uiManager.PlayNextUIButtonClickedAction += NextLevel;
        _uiManager.RestartUIButtonClickedAction += RestartLevel;
        _uiManager.GamePausedAction += PauseStatus;
    }

    private void OnDisable()
    {
        _uiManager.PlayNextUIButtonClickedAction -= NextLevel;
        _uiManager.RestartUIButtonClickedAction -= RestartLevel;
        _uiManager.GamePausedAction -= PauseStatus;
    }
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Item") == null && !_isLevelCompleted)
        {
            //Invoke Level Complete UI
            LevelCompleteAction?.Invoke();
            _isLevelCompleted = true;
            if (_isLevelCompleted)
            {
                _totalTimeTakenToCompleteLevel = _totalTime - _currentTime;
                //Debug.Log("Total time taken to complete the level: " + _totalTimeTakenToCompleteLevel
                float remainingTime = _totalTime - _totalTimeTakenToCompleteLevel;
                float percentRemaining = (remainingTime / _totalTime) * 100;
                
                //invoke star sequence
                StarAchievedAction?.Invoke(percentRemaining);
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
            RemainingTimeSendToUIAction?.Invoke(_currentTime);
        }
        else
        {
            if (!_isLevelCompleted)
            {
               //invoke on level failed
               LevelFailedAction?.Invoke();
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

    private void PauseStatus(bool pauseStatus)
    {
        _isGamePaused = pauseStatus;
    }
    
}
