using UnityEngine;
using Zenject;

public class TripleMatchInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RemainingItemManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridGenerator>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputSystem_DragAndDrop>().FromComponentInHierarchy().AsTransient();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IMergeAble>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CanvasGroup>().FromComponentInHierarchy().AsTransient();
        Container.Bind<AudioSource>().FromComponentSibling().AsTransient();
        Container.Bind<ParticleSystem>().FromComponentInChildren().AsTransient();
        Container.Bind<RectTransform>().FromComponentSibling().AsTransient();
    }
}