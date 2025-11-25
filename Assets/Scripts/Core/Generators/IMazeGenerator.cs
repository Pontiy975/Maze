using UnityEngine;

namespace Maze.Core.Generators
{
    public interface IMazeGenerator
    {
        public void Generate(MazeNode[,] grid, Vector2Int size);
    }
}
