using UnityEngine;

namespace Maze.Player.Components
{
    public enum MovementDirection
    {
        Up = 1,
        Down = -1,
        Right = 2,
        Left = -2,
        None = 0
    }

    public class PlayerMovementComponent : MonoBehaviour
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";

        public MovementDirection Direction { get; private set; } = MovementDirection.None;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            float horizontal = Input.GetAxisRaw(HORIZONTAL_AXIS);
            float vertical = Input.GetAxisRaw(VERTICAL_AXIS);

            if (horizontal == 0 && vertical == 0)
            {
                Direction = MovementDirection.None;
                return;
            }

            if (vertical != 0)
            {
                Direction = vertical > 0 ? MovementDirection.Up : MovementDirection.Down;
                return;
            }

            if (horizontal != 0)
                Direction = horizontal > 0 ? MovementDirection.Right : MovementDirection.Left;
        }
    }
}
