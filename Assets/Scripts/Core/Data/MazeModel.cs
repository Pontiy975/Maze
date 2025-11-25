using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core.Data
{
    [CreateAssetMenu(fileName = "MazeModel", menuName = "ScriptableObjects/MazeData/MazeModel")]
    public class MazeModel : ScriptableObject
    {
        [field: SerializeField] public Vector2Int Size { get; private set; } = new(10, 10);
        [field: SerializeField] public int ExitsCount { get; private set; } = 3;
    }
}
