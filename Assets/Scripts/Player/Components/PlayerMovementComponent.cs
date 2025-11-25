using Maze.Player.Data;
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

        private PlayerModel _model;
        private Transform _transform;

        public MovementDirection Direction { get; private set; } = MovementDirection.None;

        private bool _isInitialized;

        public void Init(PlayerModel model, Transform transform)
        {
            _model = model;
            _transform = transform;
            _isInitialized = true;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!_isInitialized)
                return;

            SetDirection();

            Vector2 direction = Direction switch
            {
                MovementDirection.Up => Vector2.up,
                MovementDirection.Down => Vector2.down,
                MovementDirection.Left => Vector2.left,
                MovementDirection.Right => Vector2.right,
                _ => Vector2.zero
            };

            _transform.Translate(_model.Speed * Time.deltaTime * direction);
        }

        private void SetDirection()
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
