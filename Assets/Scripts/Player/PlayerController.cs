using Maze.Core;
using Maze.Player.Components;
using Maze.Player.Data;
using UnityEngine;
using Zenject;

namespace Maze.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerModel model;
        [SerializeField] private PlayerMovementComponent movementComponent;
        [SerializeField] private PlayerAnimatorComponent animatorComponent;

        [Inject] private MazeController _mazeController;

        private MovementDirection _lastDirection = MovementDirection.None;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            _transform.position = _mazeController.CentralNode.transform.position;

            movementComponent.Init(model);
        }

        private void Update()
        {
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            MovementDirection current = movementComponent.Direction;

            if (current != _lastDirection)
            {
                animatorComponent.SetDirection((int)current);
                _lastDirection = current;
            }
        }
    }
}
