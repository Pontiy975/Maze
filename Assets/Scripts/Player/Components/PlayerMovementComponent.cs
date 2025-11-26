using Maze.Core;
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
        #region Const
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";
        private const float RAYCAST_DISTANCE = 0.25f;
        #endregion

        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private LayerMask groundLayer;

        private PlayerModel _model;
        private Transform _transform;
        private bool _isInitialized;

        #region Properties
        public MovementDirection Direction { get; private set; } = MovementDirection.None;
        public MazeNode CurrentNode { get; private set; }
        #endregion

        private readonly RaycastHit2D[] _forwardHits = new RaycastHit2D[1];
        private readonly RaycastHit2D[] _downHits = new RaycastHit2D[1];

        private void Awake()
        {
            _transform = transform;
        }

        public void Init(PlayerModel model)
        {
            _model = model;
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

            if (Direction == MovementDirection.None)
                return;

            Vector2 direction = GetDirectionVector();
            
            if (RaycastForward(direction))
                return;
            
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

        private Vector2 GetDirectionVector()
        {
            return Direction switch
            {
                MovementDirection.Up => Vector2.up,
                MovementDirection.Down => Vector2.down,
                MovementDirection.Left => Vector2.left,
                MovementDirection.Right => Vector2.right,
                _ => Vector2.zero
            };
        }

        private bool RaycastForward(Vector2 direction)
        {
            return Physics2D.RaycastNonAlloc(_transform.position, direction, _forwardHits, RAYCAST_DISTANCE, wallLayer) > 0;
        }
    }
}
