using Maze.Core;
using Maze.Core.Data;
using Maze.Core.Factories;
using Maze.Core.Generators;
using UnityEngine;
using Zenject;

public class MazeInstaller : MonoInstaller
{
    [SerializeField] private MazeModel mazeModel;
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private MazeController controllerPrefab;

    public override void InstallBindings()
    {
        switch (mazeModel.Algorithm)
        {
            case MazeAlgorithm.BinaryTree:
                Container.Bind<IMazeGenerator>().To<BinaryTreeMazeGenerator>().AsTransient();
                break;
            case MazeAlgorithm.Prims:
                Container.Bind<IMazeGenerator>().To<PrimsMazeGenerator>().AsTransient();
                break;
            default:
                Container.Bind<IMazeGenerator>().To<DFSMazeGenerator>().AsTransient();
                break;
        }

        Container.Bind<MazeNode>().FromInstance(nodePrefab).AsSingle();
        Container.Bind<IMazeNodeFactory>().To<MazeNodeFactory>().AsSingle();

        Container.Bind<MazeController>()
                 .FromComponentInNewPrefab(controllerPrefab)
                 .AsSingle()
                 .NonLazy();
    }
}