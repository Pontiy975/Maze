using Maze.Core;
using Maze.Player.Components;
using UnityEngine;
using Zenject;

namespace Maze
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementComponent movementComponent;
        [SerializeField] private PlayerAnimatorComponent animatorComponent;

        [Inject] private MazeController _mazeController;

        private MovementDirection _lastDirection = MovementDirection.None;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            _transform.position = _mazeController.CentralNode.transform.position;
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
