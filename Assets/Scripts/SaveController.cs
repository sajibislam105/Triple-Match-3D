using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SaveController : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    private void OnEnable()
    {
        _signalBus.Subscribe<TripleMatchSignals.SaveLevelSignal>(OnSaveSignal);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<TripleMatchSignals.SaveLevelSignal>(OnSaveSignal);
    }

    void OnSaveSignal()
    {
        string activeLevel =  "Level"/*SceneManager.GetActiveScene().name*/;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //PlayerPrefs.SetString("LevelSaved",activeLevel);
        PlayerPrefs.SetInt(activeLevel,currentSceneIndex);
        
        //Debug.Log(activeLevel);
    }
}
