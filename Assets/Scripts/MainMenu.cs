using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelNumber;
    
    [Inject] private AudioSource _audioSource;

    private void Start()
    {
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
