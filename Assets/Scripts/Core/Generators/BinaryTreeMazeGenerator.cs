using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core.Generators
{
    public class BinaryTreeMazeGenerator : IMazeGenerator
    {
        public void Generate(MazeNode[,] grid, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    List<MazeNode> neighbors = new();

                    if (y < size.y - 1) neighbors.Add(grid[x, y + 1]);
                    if (x < size.x - 1) neighbors.Add(grid[x + 1, y]);

                    if (neighbors.Count > 0)
                    {
                        MazeNode chosen = neighbors[Random.Range(0, neighbors.Count)];
                        grid[x, y].Connect(chosen);
                    }
                }
            }
        }
    }
}
