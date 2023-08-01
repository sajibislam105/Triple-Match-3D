
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        _audioSource.Play();
        Invoke("OnPlayGame",_audioSource.clip.length + 0.5f);
    }

    public void Quit()
    {
        _audioSource.Play();
        Application.Quit();
    }

    void OnPlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
