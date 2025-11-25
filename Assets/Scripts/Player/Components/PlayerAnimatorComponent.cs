using UnityEngine;

namespace Maze.Player.Components
{
    public class PlayerAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private static readonly int _directionID = Animator.StringToHash("Direction");

        public void SetDirection(int direction) => animator.SetInteger(_directionID, direction);
    }
}
