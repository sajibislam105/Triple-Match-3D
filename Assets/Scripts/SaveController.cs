using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SaveController : MonoBehaviour
{
    [Inject ]private LevelManager _levelManager;
    
    private void OnEnable()
    {
        _levelManager.SaveLevelAction += OnSaveAction;
    }

    private void OnDisable()
    {
        _levelManager.SaveLevelAction -= OnSaveAction;
    }

    void OnSaveAction()
    {
        string activeLevel =  "Level"/*SceneManager.GetActiveScene().name*/;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //PlayerPrefs.SetString("LevelSaved",activeLevel);
        PlayerPrefs.SetInt(activeLevel,currentSceneIndex);
        
        //Debug.Log(activeLevel);
    }
}
