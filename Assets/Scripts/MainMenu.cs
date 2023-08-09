using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource _audioSource;
    
   // [SerializeField] private Button playButton;
   // [SerializeField] private Button quitButton;
   // [SerializeField] private Button PlayAgainButton;
   [SerializeField] private TextMeshProUGUI LevelNumber;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Level"))
        {
            LevelNumber.text = PlayerPrefs.GetInt("Level").ToString();
        }
        else
        {
            LevelNumber.text = "1";
        }
        
    }

    public void PlayGame()
    {
        _audioSource.Play();
        Invoke("OnPlayGame",_audioSource.clip.length);
    }

    public void Quit()
    {
        _audioSource.Play();
        Application.Quit();
    }

    void OnPlayGame()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            int LevelToLoad = PlayerPrefs.GetInt("Level");
            SceneManager.LoadScene(LevelToLoad + 1); // will load the next level
        }
        else
        {
            //Debug.Log("No saved data");
            SceneManager.LoadScene(1);    
        }
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
