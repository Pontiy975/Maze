using Maze.Game;
using Maze.Player;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private PlayerController playerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<GameManager>()
                 .FromComponentInNewPrefab(gameManagerPrefab)
                 .AsSingle()
                 .NonLazy();

        Container.Bind<PlayerController>()
                 .FromComponentInNewPrefab(playerPrefab)
                 .AsSingle()
                 .NonLazy();
    }
}