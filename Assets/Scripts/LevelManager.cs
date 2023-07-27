using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private Button backButton;
    
    [SerializeField] private Button backToPlay;
    [SerializeField] private Button landingPage;
    [SerializeField] private Button close;

    [SerializeField] private GameObject _LevelComplete;
    [SerializeField] private GameObject _LevelFailed;
    [SerializeField] private GameObject _Pausemenu;

    private bool _isPaused;
    private bool _isLevelCompleted;
    private int _levelCount;

    private float _totalTime = 10.0f; //in seconds
    private float _currentTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        _levelCount = SceneManager.GetActiveScene().buildIndex;
        _currentTime = _totalTime;
        LevelText.text = "Level: " + _levelCount;
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Fruit") == null)
        {
            _isLevelCompleted = true;
            _LevelComplete.SetActive(true);
            LevelText.enabled = false;
            TimerText.enabled = false;
        }

        if (!_isPaused)
        {
            Timer();
        }
        
    }

    void Timer()
    {
        if (_currentTime >= 0)
        {
            _currentTime -= Time.deltaTime; 
            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            if (!_isLevelCompleted)
            {
                _LevelFailed.SetActive(true);
                LevelText.enabled = false;
                TimerText.text = "Time Finished";                
            }
             
        }
    }

    public void OnPlayButtonClicked()
    {
        audioSource.Play();
        Invoke("NextLevel", audioSource.clip.length + 1f);
    }
    public void onRestartButtonClicked()
    {
        audioSource.Play();
        Invoke("RestartLevel", audioSource.clip.length + 1f); 
    }
    
    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        
    }
    
    void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        
    }

    public void onBackButtonClicked()
    {
        LevelText.enabled = false;
        TimerText.enabled = false;
        _isPaused = true;
        _Pausemenu.SetActive(true);
    }

    public void OnPauseMenuPlayButtonClicked()
    {
        LevelText.enabled = true;
        TimerText.enabled = true;
        _isPaused = false;
        _Pausemenu.SetActive(false);
    }

    public void OnPauseMenuLandingPageButtonClicked()
    {
        SceneManager.LoadScene(0); // 0 means Landing Page
    }
    
    public void OnPauseMenuCloseButtonClicked()
    {
        LevelText.enabled = true;
        TimerText.enabled = true;
        _isPaused = false;
        _Pausemenu.SetActive(false);
    }
    

    
}
