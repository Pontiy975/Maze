using Maze.Core;
using UnityEngine;
using Zenject;

public class MazeInstaller : MonoInstaller
{
    [SerializeField] private MazeController controllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<MazeController>()
                 .FromComponentInNewPrefab(controllerPrefab)
                 .AsSingle()
                 .NonLazy();
    }
}