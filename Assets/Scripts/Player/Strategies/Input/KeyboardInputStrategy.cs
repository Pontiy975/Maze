using Maze.Player.Components;

namespace Maze.Player.Strategies.Input
{
    public class KeyboardInputStrategy : IPlayerInputStrategy
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";

        public MovementDirection GetDirection()
        {
            float horizontal = UnityEngine.Input.GetAxisRaw(HORIZONTAL_AXIS);
            float vertical = UnityEngine.Input.GetAxisRaw(VERTICAL_AXIS);

            if (vertical != 0)
                return vertical > 0 ? MovementDirection.Up : MovementDirection.Down;

            if (horizontal != 0)
                return horizontal > 0 ? MovementDirection.Right : MovementDirection.Left;

            return MovementDirection.None;
        }
    }
}
