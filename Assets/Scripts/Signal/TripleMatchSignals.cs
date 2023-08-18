
using UnityEngine;

public class TripleMatchSignals
{
    public class ObjectDroppingOnCellSignal
    {
        public Item ToDragItem;
    }

    public class InstructionStatusSignal
    {
        //null parameter
    }

    public class ScaleDownObjectSignal
    {
        public Transform ToDrag;
    }

    //UI manager signals
    public class RestartUIButtonClickedSignal
    {
        //null parameter
    }

    public class PlayNextUIButtonClickedSignal
    {
        //null parameter
    }

    public class GamePausedSignal
    {
        public bool IsPaused;
    }

    //Level Manager Signals
    /*public Action LevelCompleteAction;
    public Action LevelFailedAction;
    public Action<float> RemainingTimeSendToUIAction;
    public Action<float> StarAchievedAction;
    public Action SaveLevelAction;*/

    public class LevelCompleteSignal
    {
        //null parameter
    }

    public class LevelFailedSignal
    {
        //null parameter
    }

    public class  RemainingTimeSendToUISignal
    {
        public float CurrentTime;
    }

    public class  StarAchievedSignal
    {
        public float PercentRemaining;
    }
    public class SaveLevelSignal
    {
        //null parameter
    }
    
}
