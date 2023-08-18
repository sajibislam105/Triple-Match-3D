using UnityEngine;
using Zenject;

public class TripleMatchInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RemainingItemManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridGenerator>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<InputSystem_DragAndDrop>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IMergeAble>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CanvasGroup>().FromComponentInHierarchy().AsTransient();
        Container.Bind<AudioSource>().FromComponentSibling().AsTransient();
        Container.Bind<ParticleSystem>().FromComponentInChildren().AsTransient();
        Container.Bind<RectTransform>().FromComponentSibling().AsTransient();
        
        SignalBusInstaller.Install(Container);
        
        //input system scripts signals
        Container.DeclareSignal<TripleMatchSignals.ObjectDroppingOnCellSignal>();
        Container.DeclareSignal<TripleMatchSignals.InstructionStatusSignal>();
        Container.DeclareSignal<TripleMatchSignals.ScaleDownObjectSignal>();
        
        //UI manager signals
        Container.DeclareSignal<TripleMatchSignals.PlayNextUIButtonClickedSignal>();
        Container.DeclareSignal<TripleMatchSignals.RestartUIButtonClickedSignal>();
        Container.DeclareSignal<TripleMatchSignals.GamePausedSignal>();
        
        //level manager signals
        Container.DeclareSignal<TripleMatchSignals.LevelCompleteSignal>();
        Container.DeclareSignal<TripleMatchSignals.LevelFailedSignal>();
        Container.DeclareSignal<TripleMatchSignals.RemainingTimeSendToUISignal>();
        Container.DeclareSignal<TripleMatchSignals.StarAchievedSignal>();
        Container.DeclareSignal<TripleMatchSignals.SaveLevelSignal>();
        
        

    }
}