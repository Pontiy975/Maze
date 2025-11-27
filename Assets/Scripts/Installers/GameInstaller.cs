using Maze.Game;
using Maze.Player;
using Maze.Player.Strategies.Input;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private CustomJoystick joystickPrefab;

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

#if UNITY_EDITOR
        Container.Bind<IPlayerInputStrategy>()
                 .To<KeyboardInputStrategy>()
                 .AsSingle();
#else
        Container.Bind<CustomJoystick>()
                 .FromComponentInNewPrefab(joystickPrefab)
                 .UnderTransform(canvasRect)
                 .AsSingle()
                 .NonLazy();

        Container.Bind<IPlayerInputStrategy>()
                 .To<JoystickInputStrategy>()
                 .AsSingle();
#endif
    }
}