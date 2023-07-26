using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject _LevelComplete;
    [SerializeField] private GameObject _LevelFailed;

    private int _levelCount;

    private float _totalTime = 10.0f; //in seconds
    private float _currentTime;

    void Start()
    {
        _levelCount = 1;
        _currentTime = _totalTime;
        LevelText.text = "Level: " + _levelCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Fruit") == null)
        {
            _LevelComplete.SetActive(true);
            LevelText.enabled = false;
            TimerText.enabled = false;
        }
        Timer();
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
            _LevelFailed.SetActive(true);
            LevelText.enabled = false;
            TimerText.text = "Time Finished"; 
        }
    }

    public void OnPlayButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void onRestartButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
