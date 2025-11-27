using Maze.Player.Components;
using UnityEngine;

namespace Maze.Player.Strategies.Input
{
    public class SwipeInputStrategy : IPlayerInputStrategy
    {
        private Vector2 _startTouch;
        private Vector2 _currentDirection;
        private bool _touchActive;

        public MovementDirection GetDirection()
        {
            _currentDirection = Vector2.zero;

            if (UnityEngine.Input.touchCount == 0)
            {
                _touchActive = false;
                return MovementDirection.None;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouch = touch.position;
                    _touchActive = true;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_touchActive)
                    {
                        Vector2 delta = touch.position - _startTouch;

                        if (delta.magnitude < 50f)
                            return MovementDirection.None;

                        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                            _currentDirection = delta.x > 0 ? Vector2.right : Vector2.left;
                        else
                            _currentDirection = delta.y > 0 ? Vector2.up : Vector2.down;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _touchActive = false;
                    _currentDirection = Vector2.zero;
                    break;
            }

            if (_currentDirection == Vector2.zero)
                return MovementDirection.None;

            return _currentDirection.y > 0 ? MovementDirection.Up :
                   _currentDirection.y < 0 ? MovementDirection.Down :
                   _currentDirection.x > 0 ? MovementDirection.Right :
                   MovementDirection.Left;
        }
    }
}
