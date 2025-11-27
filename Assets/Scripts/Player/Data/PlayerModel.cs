using UnityEngine;

namespace Maze.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerModel", menuName = "ScriptableObjects/PlayerData/PlayerModel")]
    public class PlayerModel : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 5f;
    }
}
