using Maze.Game;
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
        [SerializeField] private ParticleSystem debrisFX;

        [Inject] private GameManager _gameManager;

        private MovementDirection _lastDirection = MovementDirection.None;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            
            movementComponent.Init(model);
            movementComponent.OnCurrentNodeChanged += OnNodeChanged;
        }

        private void OnDestroy()
        {
            movementComponent.OnCurrentNodeChanged -= OnNodeChanged;
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

                if (current == MovementDirection.None)
                    debrisFX.Stop();
                else
                    debrisFX.Play();
            }
        }

        private void OnNodeChanged()
        {
            _gameManager.AddNode(movementComponent.CurrentNode);
        }
    }
}
