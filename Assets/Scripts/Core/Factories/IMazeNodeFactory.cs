using UnityEngine;

namespace Maze.Core.Factories
{
    public interface IMazeNodeFactory
    {
        public MazeNode Create(Vector2Int gridSize, Vector2Int gridPosition, Transform parent);
    }
}
