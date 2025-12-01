using Maze.Core;
using Maze.Game;
using Maze.Player.Data;
using Maze.Player.Strategies.Input;
using System;
using UnityEngine;
using Zenject;

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
        public event Action OnCurrentNodeChanged;

        private const float RAYCAST_DISTANCE = 0.18f;

        [SerializeField] private GameStateMachine gameStateMachine;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private LayerMask groundLayer;

        [Inject] private MazeController _mazeController;
        [Inject] private IPlayerInputStrategy _inputStrategy;

        private PlayerModel _model;
        private Transform _transform;
        private bool _isInitialized;

        #region Properties
        public MovementDirection Direction { get; private set; } = MovementDirection.None;
        public MazeNode CurrentNode { get; private set; }
        #endregion

        private readonly RaycastHit2D[] _wallHits = new RaycastHit2D[1];

        private void Awake()
        {
            _transform = transform;
         
            GameManager.GetPlayerPosition += GetPlayerPosition;
            GameManager.GetWorldPlayerPosition += GetWorldPlayerPosition;
            GameManager.OnPlayerLoaded += OnPlayerLoaded;
            _mazeController.OnMazeInitialized += OnMazeInitialized;
        }

        private void OnDestroy()
        {
            GameManager.GetPlayerPosition -= GetPlayerPosition;
            GameManager.GetWorldPlayerPosition -= GetWorldPlayerPosition;
            GameManager.OnPlayerLoaded -= OnPlayerLoaded;
            _mazeController.OnMazeInitialized -= OnMazeInitialized;
        }

        private void Update()
        {
            if (!gameStateMachine.CheckStates(GameState.InGame))
            {
                if (Direction != MovementDirection.None)
                    Direction = MovementDirection.None;

                return;
            }

            Move();
        }

        public void Init(PlayerModel model)
        {
            _model = model;
            _isInitialized = true;
        }

        private void Move()
        {
            if (!_isInitialized)
                return;

            Direction = _inputStrategy.GetDirection();

            if (Direction == MovementDirection.None)
                return;

            Vector2 dir = GetDirectionVector();

            if (RaycastForward(dir))
                return;

            _transform.Translate(_model.Speed * Time.deltaTime * dir);
            DetectNode();
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
            return Physics2D.RaycastNonAlloc(_transform.position, direction, _wallHits, RAYCAST_DISTANCE, wallLayer) > 0;
        }

        private void DetectNode()
        {
            if (_mazeController.TryGetNodeAtWorldPosition(_transform.position, out MazeNode node))
            {
                if (CurrentNode != node)
                {
                    CurrentNode = node;
                    OnCurrentNodeChanged?.Invoke();
                }
            }
            else
            {
                CurrentNode = null;
                OnCurrentNodeChanged?.Invoke();
            }
        }

        private void OnMazeInitialized()
        {
            _transform.position = _mazeController.CentralNode.transform.position;
        }

        private void OnPlayerLoaded(Vector2Int position)
        {
            CurrentNode = _mazeController.GetNode(position);
            _transform.position = CurrentNode.transform.position;
        }

        private Vector2Int GetPlayerPosition() => CurrentNode ? CurrentNode.Position : Vector2Int.zero;
        private Vector2 GetWorldPlayerPosition() => _transform.position;
    }
}
