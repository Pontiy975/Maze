using Maze.Player.Components;

namespace Maze.Player.Strategies.Input
{
    public interface IPlayerInputStrategy
    {
        public MovementDirection GetDirection();
    }
}
