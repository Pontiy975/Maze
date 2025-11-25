using UnityEngine;

namespace Maze.Core.Data
{
    [CreateAssetMenu(fileName = "MazeModel", menuName = "ScriptableObjects/MazeData/MazeModel")]
    public class MazeModel : ScriptableObject
    {
        [field: SerializeField] public Vector2Int Size { get; private set; } = new(10, 10);
        [field: SerializeField, Min(1)] public int ExitsCount { get; private set; } = 3;
    }
}
