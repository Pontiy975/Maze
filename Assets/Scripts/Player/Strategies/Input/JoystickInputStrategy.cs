using Maze.Player.Components;
using UnityEngine;
using Zenject;

namespace Maze.Player.Strategies.Input
{
    public class JoystickInputStrategy : IPlayerInputStrategy
    {
        [Inject] private CustomJoystick _joystick;

        public MovementDirection GetDirection()
        {
            if (!_joystick || !_joystick.HasInput)
                return MovementDirection.None;

            Vector3 dir = _joystick.DirectionXZ;

            if (dir.sqrMagnitude < 0.1f)
                return MovementDirection.None;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
                return dir.x > 0 ? MovementDirection.Right : MovementDirection.Left;

            return dir.z > 0 ? MovementDirection.Up : MovementDirection.Down;
        }
    }
}
